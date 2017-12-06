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
using System.Collections.ObjectModel;

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
        public IActionResult Index()
        {
            return View();
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

<<<<<<< HEAD
        public List<T> PopulateTypedObject<T>(string kind_id, JArray json)
=======
        public T ResponseObjectBuilder<T>(string responseBody)
>>>>>>> 9953640a60b8459b9978d44c252208641583e257
        {

            List<T> list = new List<T>();
            for (int i = 0; i < json.Count; i++)
            {
                

                foreach (JObject item in json[i]["data"]["children"])
                {
                    Child c = item.ToObject<Child>();
                    if(c.kind == kind_id)
                    {
                        var hest = item["data"].ToObject<T>();
                        list.Add(hest);
                    }
                }
            }
            return list;
        }

        public ICommentable ResponseObjectBuilder<T>(string responseBody)
        {

            JArray json = JArray.Parse(responseBody);
            
            if (typeof(T) == typeof(Comment))
            {

                throw new NotImplementedException();
                new Comment()
                {

                };
            }

            if (typeof(T) == typeof(Post)) {

                List<Comment> Commentslist = PopulateTypedObject<Comment>("t1", json);
                List<Post> Postlist = PopulateTypedObject<Post>("t3", json);
                Post post = Postlist[0];
                post.comments = new ObservableCollection<Comment>(Commentslist);
                return post;
            }
            if (typeof(T) == typeof(Subreddit))
            {
                List<Post> PostsList = PopulateTypedObject<Post>("t3", json);
                //List<Subreddit> list2 = PopulateTypedObject<Post>("t3", json);
                Subreddit subreddit = new Subreddit(new ObservableCollection<Post>(PostsList));

                return subreddit;
            }
            else return null;
            //OMFORM NODE LISTEN TIL OBJECT FORMAT PÅ BAGGRUND AF 
        }
<<<<<<< HEAD

=======
        
>>>>>>> 9953640a60b8459b9978d44c252208641583e257
        public async Task<T> ResponseJsonBuilderAsync<T>(HttpResponseMessage response)
        {

            var obj = Activator.CreateInstance(typeof(T));

            string responseBody = await response.Content.ReadAsStringAsync();
            //string jResponseBody = JsonConvert.SerializeObject(responseBody);
            ResponseObjectBuilder<T>(responseBody);

            return (T)obj;
        }

        public Task<(HttpStatusCode, string)> LoginToReddit(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}