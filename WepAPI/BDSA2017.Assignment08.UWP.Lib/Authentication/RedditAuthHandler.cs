using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Windows.Storage;
using UI.Lib.Model.RedditRestInterfaces;

namespace UI.Lib.Authentication
{
    public class RedditAuthHandler : IRedditAuthHandler
    {
        private string _token;
        private string _refreshToken;
        private readonly ApplicationDataContainer _appSettings;
        private const string AccessTokenUrl = "https://www.reddit.com/api/v1/access_token";
        private const string ClientId = "ephxxGR7ZA77nA";
        private readonly IRedditApiConsumer _consumer;
        public RedditAuthHandler(IRedditApiConsumer consumer)
        {
            _consumer = consumer;
            _appSettings = ApplicationData.Current.RoamingSettings;
        }
        public string StartUrl = "https://www.reddit.com/api/v1/authorize?client_id=ephxxGR7ZA77nA&response_type=code&state=assasdsdadsa4125&redirect_uri=https://gorillaapi.azurewebsites.net/&duration=permanent&scope=*";
        public string EndUrl = "https://gorillaapi.azurewebsites.net/";

        public async Task BeginAuth()
        {
         
            var appSettingsValue = _appSettings.Values["reddit_refresh_token"];

            if (appSettingsValue != null) //If the refresh token exists, just login right away
            {
                _refreshToken = appSettingsValue.ToString();
                await RefreshToken();
            }
            else //Otherwise.. show the user the login panel
            { 
                var startUri = new Uri(StartUrl);
                var endUri = new Uri(EndUrl);
                var webAuthenticationResult =
                    await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(
                        Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
                        startUri,
                        endUri);

                //Force authentication
                if (webAuthenticationResult.ResponseStatus == Windows.Security.Authentication.Web.WebAuthenticationStatus.Success)
                {
                    var result = webAuthenticationResult.ResponseData;
                    await Authenticate(result);
                }
                else
                {
                    await BeginAuth();
                }
            }
        }
        public HttpRequestMessage AuthenticateRequest(HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);
            return request;
        }
        public async Task<JToken> SendRequest(string body)
        {
            var uri = new Uri(AccessTokenUrl);

            var request = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = new HttpMethod("POST")
            };
            request.Headers.Add("User-Agent", "Gorilla");
            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientId + ":"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            request.Content = new StringContent(body,
                Encoding.UTF8,
                "application/x-www-form-urlencoded");
            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);
                try
                {
                    var contentBody = JToken.Parse(await response.Content.ReadAsStringAsync());
                    if (contentBody["error"] == null)
                    {
                        return contentBody;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Could not login " + e.Message);
                }
            }
            return null;
        }

        public void LogOut()
        {
            _appSettings.Values.Remove("reddit_refresh_token");
        }

        public async Task RefreshToken()
        {
           
            var contentBody = await SendRequest($"grant_type=refresh_token&refresh_token={_refreshToken}");
            _token = contentBody["access_token"].ToObject<string>();
            _consumer.Authenticate(this);
        }
        public async Task Authenticate(string data)
        {
            var code = GetAuthCode(data);
        
            var contentBody = await SendRequest($"grant_type=authorization_code&code={code}&redirect_uri=https://gorillaapi.azurewebsites.net/");
            _token = contentBody["access_token"].ToObject<string>();
            _refreshToken = contentBody["refresh_token"].ToObject<string>();
            _appSettings.Values["reddit_refresh_token"] = _refreshToken;
            _consumer.Authenticate(this);
        }

        public string GetAuthCode(string data)
        {
            //Split the redirect url into bits
            var urlparamsString = data.Split('?')[1];
            var paramsStrings = urlparamsString.Split('&');
            //Build Authenticate JSON object out of the bits
            var jsonString = paramsStrings.Select(s => s.Split('=')).Aggregate("{", (current, split) => current + $"'{split[0]}':'{split[1]}',");
            jsonString += "}";
            var jsonObject = JToken.Parse(jsonString);

            //Check if everything is OK
            if (jsonObject["error"] != null) throw new Exception("Could not auth!");

            if (jsonObject["code"] == null) throw new Exception("Could not find auth code!");

            _token = jsonObject["code"].ToObject<string>();
            return _token;
        }
    }
}
