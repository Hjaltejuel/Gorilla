using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Entities.RedditEntities;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using UITEST;
using Windows.Storage;
using UITEST.RedditInterfaces;

namespace UITEST.Authentication
{
    public class RedditAuthHandler
    {
        private string token;
        private string refresh_token;
        private readonly ApplicationDataContainer _appSettings;
        private const string AccessTokenUrl = "https://www.reddit.com/api/v1/access_token";
        private const string Client_id = "ephxxGR7ZA77nA";
        public RedditAuthHandler()
        {
            _appSettings = ApplicationData.Current.RoamingSettings;
        }
        public string startURL = "https://www.reddit.com/api/v1/authorize?client_id=ephxxGR7ZA77nA&response_type=code&state=assasdsdadsa4125&redirect_uri=https://gorillaapi.azurewebsites.net/&duration=permanent&scope=*";
        public string endURL = "https://gorillaapi.azurewebsites.net/";

        public async Task BeginAuth()
        {
            refresh_token = _appSettings.Values["reddit_refresh_token"].ToString();

            if (refresh_token != null) //If the refresh token exists, just login right away
            {
                await RefreshToken();
            }
            else
            { //Otherwise.. show the user the login panel
                Uri startURI = new Uri(startURL);
                Uri endURI = new Uri(endURL);
                string result;
                var webAuthenticationResult =
                    await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(
                        Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
                        startURI,
                        endURI);

                //Force authentication
                if (webAuthenticationResult.ResponseStatus == Windows.Security.Authentication.Web.WebAuthenticationStatus.Success)
                {
                    result = webAuthenticationResult.ResponseData.ToString();
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
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            return request;
        }

        public async Task RefreshToken()
        {
            var consumer = App.ServiceProvider.GetService<IRedditAPIConsumer>();
            Uri uri = new Uri(AccessTokenUrl);

            var request = new HttpRequestMessage() { RequestUri = uri };
            request.Method = new HttpMethod("POST");
            request.Headers.Add("User-Agent", "Gorilla");
            var BasicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(Client_id + ":"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", BasicAuth);
            request.Content = new StringContent($"grant_type=refresh_token&refresh_token={refresh_token}",
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
                        token = contentBody["access_token"].ToObject<string>();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Could not login " + e.Message);
                }
            }
            consumer.Authenticate(this);
        }
        
        public async Task Authenticate(string data)
        {
            var code = GetAuthCode(data);
            var consumer = App.ServiceProvider.GetService<IRedditAPIConsumer>();

            Uri uri = new Uri(AccessTokenUrl);

            var request = new HttpRequestMessage() { RequestUri = uri };
            request.Method = new HttpMethod("POST");
            request.Headers.Add("User-Agent", "Gorilla");
            var BasicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(Client_id + ":"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", BasicAuth);
            request.Content = new StringContent($"grant_type=authorization_code&code={code}&redirect_uri=https://gorillaapi.azurewebsites.net/",
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
                        token = contentBody["access_token"].ToObject<string>();
                        refresh_token = contentBody["refresh_token"].ToObject<string>();

                        _appSettings.Values["reddit_refresh_token"] = refresh_token;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Could not login " + e.Message);
                }
            }
            consumer.Authenticate(this);
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

            token = jsonObject["code"].ToObject<string>();
            return token;
        }
    }
}
