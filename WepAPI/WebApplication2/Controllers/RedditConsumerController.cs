using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;

namespace WebApplication2.Controllers
{
    public class RedditConsumerController : IRedditAPIConsumer
    {

        public string BaseUrl { get => "https://reddit.com/"; set => throw new NotImplementedException(); }

        //Brug enums til sortBy? 
        public async Task<HttpResponseMessage> Get(Uri uri)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
               

                return response;

            }
        }

        //CONVENTION: IF SORTBY STRING IS EMPTY, HOT IS DEFAULTED TO
        public async Task<Subreddit> GetSubredditAsync(string subredditName, string sortBy)
        {
            if (sortBy.Equals("")) sortBy = "hot";

            Uri uri = new Uri(string.Format
               (BaseUrl +
               "r/{0}/{1}/.json?json_raw=1",
               subredditName,
               sortBy));
            HttpResponseMessage response = await Get(uri);
            response.EnsureSuccessStatusCode();
            //if it goes wrong do this...

            return await ResponseJsonBuilderAsync<Subreddit>(response);

        }
        //t3 = comments on link / post & t5 = subreddit & t1 = comment
        public async Task<Post> GetPostAsync(string name_id)
        {

            Uri uri = new Uri(string.Format(BaseUrl + "comments/{0}.json?json_raw=1", name_id));

            HttpResponseMessage response = await Get(uri);
            response.EnsureSuccessStatusCode();


            return await ResponseJsonBuilderAsync<Post>(response);
        }


        public async Task<HttpResponseMessage> Post(Uri uri, Object o)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = uri;
                //VED IKKE OM DET VIRKER SÅDAN HER

                JToken jToken = JToken.FromObject(o);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage request = await client.PostAsJsonAsync<JToken>(client.BaseAddress, jToken);

                return request;
            }
        }

        public async Task<(HttpStatusCode, string)> PostPostAsync(Post p)
        {
            Uri uri = new Uri(BaseUrl + "api/submit");
            HttpResponseMessage response = await Post(uri, p);

            return await (ResponseStatusAsync(response));
        }

        public async Task<(HttpStatusCode, string)> PostCommentAsync(Comment c)
        {
            Uri uri = new Uri(BaseUrl + "api/comment");
            HttpResponseMessage response = await Post(uri, c);

            return await (ResponseStatusAsync(response));
        }

        public async Task<(HttpStatusCode, string)> PostVoteAsync(Vote v)
        {
            Uri uri = new Uri(BaseUrl + "api/vote");
            HttpResponseMessage response = await Post(uri, v);

            return await (ResponseStatusAsync(response));
        }
        public async Task<(HttpStatusCode, string)> ResponseStatusAsync(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            HttpStatusCode statusCode = response.StatusCode;

            return (statusCode, responseBody);
        }

        public T ResponseObjectBuilder<T>(string responseBody)
        {
            JObject json = JObject.Parse(responseBody);

            var value = (string)json[""]["data"]["children"];
            List<Child> deserialized = JsonConvert.DeserializeObject<List<Child>>(responseBody);
            Post p = new Post();
            //OMFORM NODE LISTEN TIL OBJECT FORMAT PÅ BAGGRUND AF T
            throw new NotImplementedException();
        }

        public async Task<T> ResponseJsonBuilderAsync<T>(HttpResponseMessage response)
        {

            var obj = Activator.CreateInstance(typeof(T));

            string responseBody = await response.Content.ReadAsStringAsync();
            //string jResponseBody = JsonConvert.SerializeObject(responseBody);
            ResponseObjectBuilder<Post>(responseBody);


            return (T)obj;
        }
    }
}