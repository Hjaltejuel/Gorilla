using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace WebApplication2.Controllers
{
    public class RedditConsumerController : Controller, IRedditAPIConsumer
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
                string responseBody = await response.Content.ReadAsStringAsync();


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

            //KALD responseJsonBuilder(response)!
            //string responseBody = await response.Content.ReadAsStringAsync();


            return null;
        }
        //t3 = comments on link / post & t5 = subreddit & t1 = comment
        public async Task<Post> GetPostAsync(string name_id)
        {

            Uri uri = new Uri(string.Format(BaseUrl + "comments/{0}.json?json_raw=1", name_id));

            HttpResponseMessage response = await Get(uri);
            response.EnsureSuccessStatusCode();

            return null;
            //KALD responseJsonBuilder(resonse)!
            //string responseBody = await response.Content.ReadAsStringAsync();

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
        public IActionResult Index()
        {
            return View();
        }

        public async Task<(HttpStatusCode, string)> PostPostAsync(Post p)
        {
            Uri uri = new Uri(BaseUrl + "api/submit");
            HttpResponseMessage response = await Post(uri, p);
            //KALD responseJsonBuilder(resonse)!
            //string responseBody = await response.Content.ReadAsStringAsync();
            return (0, null);
        }

        public async Task<(HttpStatusCode, string)> PostCommentAsync(Comment c)
        {
            Uri uri = new Uri(BaseUrl + "api/comment");
            HttpResponseMessage response = await Post(uri, c);

            //KALD responseJsonBuilder(resonse)!
            //string responseBody = await response.Content.ReadAsStringAsync();
            return (0, null);
        }

        public async Task<(HttpStatusCode, string)> PostVoteAsync(Vote v)
        {
            Uri uri = new Uri(BaseUrl + "api/vote");
            HttpResponseMessage response = await Post(uri, v);


            //KALD responseJsonBuilder(resonse)!
            return (0, null);
        }
        public async Task<(HttpStatusCode, string)> ResponseJsonBuilder(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            return (HttpStatusCode.Accepted,"");
        }
    }
}