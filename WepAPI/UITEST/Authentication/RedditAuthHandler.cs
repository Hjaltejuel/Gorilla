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

namespace Gorilla.Authentication
{
    class RedditAuthHandler
    {
        private string token;
        string refresh_token;
        public RedditAuthHandler()
        {
        }
        public string startURL = "https://www.reddit.com/api/v1/authorize?client_id=ephxxGR7ZA77nA&response_type=code&state=assasdsdadsa4125&redirect_uri=https://gorillaapi.azurewebsites.net/&duration=permanent&scope=*";
        public string endURL = "https://gorillaapi.azurewebsites.net/";
        
        public async Task BeginAuth()
        {
            Uri startURI = new Uri(startURL);
            Uri endURI = new Uri(endURL);

            string result;

            try
            {
                var webAuthenticationResult =
                    await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(
                        Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
                        startURI,
                        endURI);

                switch (webAuthenticationResult.ResponseStatus)
                {
                    case Windows.Security.Authentication.Web.WebAuthenticationStatus.Success:
                        // Successful authentication.
                        result = webAuthenticationResult.ResponseData.ToString();
                        await AuthenticateAPIConsumer(result);
                        break;
                    case Windows.Security.Authentication.Web.WebAuthenticationStatus.ErrorHttp:
                        // HTTP error. 
                        result = webAuthenticationResult.ResponseErrorDetail.ToString();
                        break;
                    default:
                        // Other error.
                        result = webAuthenticationResult.ResponseData.ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                // Authentication failed. Handle parameter, SSL/TLS, and Network Unavailable errors here. 
                result = ex.Message;
            }

        }

        public async Task AuthenticateAPIConsumer(string data)
        {
            var code = GetAuthCode(data);
            var consumer = App.ServiceProvider.GetService<IRedditAPIConsumer>();
            await consumer.Authenticate(code);
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
