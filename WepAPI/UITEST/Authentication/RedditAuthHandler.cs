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

        public async Task Authenticate(string data)
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

            return jsonObject["code"].ToObject<string>();
        }
    }
}
