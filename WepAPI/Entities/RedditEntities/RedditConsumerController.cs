using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.ObjectModel;

namespace Entities.RedditEntities    
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
                string responseBody = await response.Content.ReadAsStringAsync();
                return response;
            }
        }
        public async Task<HttpResponseMessage> Post(Uri uri, Object o)
        {
            using (var client = new HttpClient())
            {
                //YAzEEEEEEEES
                //12341234
                //WUF6RUVFRUVFRUVTOjEyMzQxMjM0
                client.BaseAddress = uri;
                //VED IKKE OM DET VIRKER SÅDAN HER
                JToken jToken = JToken.FromObject(o);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage request = await client.PostAsJsonAsync<JToken>(client.BaseAddress, jToken);
                return request;
            }
        }
        
        //CONVENTION: IF SORTBY STRING IS EMPTY, HOT IS DEFAULTED TO
        public async Task<Subreddit> GetSubredditAsync(string subredditName, string sortBy="hot")
        {
            Uri aboutUri = new Uri(string.Format
               (BaseUrl +
               "r/{0}/about.json?json_raw=1",
               subredditName,
               sortBy));

            HttpResponseMessage response = await Get(aboutUri);
            string responseBody = await response.Content.ReadAsStringAsync();
            ChildNode listings = JObject.Parse(responseBody).ToObject<ChildNode>();
            Subreddit subreddit = listings.data.ToObject<Subreddit>();
            


            Uri postsUri = new Uri(string.Format
               (BaseUrl +
               "r/{0}/{1}/.json?json_raw=1",
               subredditName,
               sortBy));

            response = await Get(postsUri);
            responseBody = await response.Content.ReadAsStringAsync();

            Listing postsListing = JObject.Parse(responseBody).ToObject<Listing>();
            subreddit.posts = new ObservableCollection<Post>();
            foreach(ChildNode child in postsListing.data.children)
            {
                subreddit.posts.Add(child.data.ToObject<Post>());
            }

            return subreddit;
        }

        //t3 = comments on link / post & t5 = subreddit & t1 = comment
        public async Task<Post> GetPostAndCommentsByIdAsync(string post_id)
        {
            Uri uri = new Uri($"{BaseUrl}/comments/{post_id}.json?json_raw=1");
            HttpResponseMessage response = await Get(uri);
            string responseBody = await response.Content.ReadAsStringAsync();
            Listing[] listings = JArray.Parse(responseBody).ToObject<Listing[]>();
            Post post = listings[0].data.children[0].data.ToObject<Post>();
            post.BuildReplies(listings[1]);
            return post;
        }

        public void DeserializeCommentListing(JToken JSONCommentListing)
        {
            
        }

        public async Task<(HttpStatusCode, string)> PostPostAsync(Post p)
        {
            Uri uri = new Uri(BaseUrl + "api/submit");
            HttpResponseMessage response = await Post(uri, p);

            return await (ResponseStatusAsync(response));
        }

        public async Task<(HttpStatusCode, string)> PostCommentAsync(Comment_old c)
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
        public Task<(HttpStatusCode, string)> LoginToReddit(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}