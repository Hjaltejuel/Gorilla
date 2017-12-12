using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace Entities.RedditEntities    
{
    public class RedditConsumerController : IRedditAPIConsumer
    {
        public string BaseUrl { get => "https://reddit.com/"; set => throw new NotImplementedException(); }

        private string token = "kPZsHAUEk6fvpfVP3nmAcrL4GpI";
        private string refresh_token = "51999737725-OYI8KJ5T56KSO4xAyvoVhA8t5TM";
        //Brug enums til sortBy? 
        public async Task<HttpResponseMessage> Get(string uri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://oauth.reddit.com/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("User-Agent", "Gorilla");
                HttpResponseMessage response = await client.GetAsync(uri);
                return response;
            }
        }
        

        private const string SslLoginUrl = "https://ssl.reddit.com/api/login";

        public async Task<HttpResponseMessage> Post(string uri, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://oauth.reddit.com/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("User-Agent", "Gorilla");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Content = content;
                HttpResponseMessage repsonse = await client.SendAsync(request);
                return repsonse;
            }
        }

        //CONVENTION: IF SORTBY STRING IS EMPTY, HOT IS DEFAULTED TO
        public async Task<Subreddit> GetSubredditAsync(string subredditName, string sortBy="hot")
        {
            string aboutUri = $"r/{subredditName}/about.json?json_raw=1";

            HttpResponseMessage response = await Get(aboutUri);
            string responseBody = await response.Content.ReadAsStringAsync();
            ChildNode listings = JObject.Parse(responseBody).ToObject<ChildNode>();
            Subreddit subreddit = listings.data.ToObject<Subreddit>();

            string postsUri = $"r/{subredditName}/{sortBy}/.json?json_raw=1";

            HttpResponseMessage response1 = await Get(postsUri);
            responseBody = await response1.Content.ReadAsStringAsync();

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
            string uri = $"{BaseUrl}/comments/{post_id}.json?json_raw=1";
            HttpResponseMessage response = await Get(uri);
            string responseBody = await response.Content.ReadAsStringAsync();
            Listing[] listings = JArray.Parse(responseBody).ToObject<Listing[]>();
            Post post = listings[0].data.children[0].data.ToObject<Post>();
            post.BuildReplies(listings[1]);
            return post;
        }
        

        //public async Task<(HttpStatusCode, string)> PostPostAsync(Post p)
        //{
        //    Uri uri = new Uri(BaseUrl + "api/submit");
        //    HttpResponseMessage response = await Post(uri, p);

        //    return await (ResponseStatusAsync(response));
        //}

        //public async Task<(HttpStatusCode, string)> PostCommentAsync(Comment_old c)
        //{
        //    Uri uri = new Uri(BaseUrl + "api/comment");
        //    HttpResponseMessage response = await Post(uri, c);

        //    return await (ResponseStatusAsync(response));
        //}

        public async Task<(HttpStatusCode, string)> ResponseStatusAsync(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            HttpStatusCode statusCode = response.StatusCode;

            return (statusCode, responseBody);
        }

        public async Task<(HttpStatusCode, string)> LoginToReddit(string username, string password)
        {
            /*
            "access_token": "daAVJCA7aA38doAt5_FcS6wR-2U",
            "token_type": "bearer",
            "expires_in": 3600,
            "refresh_token": "51999737725-OYI8KJ5T56KSO4xAyvoVhA8t5TM",
            "scope": "*"
            */
            
            return (0,null);
        }

        public async Task<User> GetAccountDetails()
        {
            HttpResponseMessage response = await Get("api/v1/me");
            User user = JToken.Parse(await response.Content.ReadAsStringAsync()).ToObject<User>();
            return user;
        }

        public Task<(HttpStatusCode, string)> PostVoteAsync(AbstractCommentable commentable, int direction)
        {
            //Uri uri = new Uri(BaseUrl + "api/vote");
            //HttpResponseMessage response = await Post(uri, 1);

            //return await(ResponseStatusAsync(response));
            throw new NotImplementedException();
        }

        public Task<(HttpStatusCode, string)> CommentOnCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task<(HttpStatusCode, string)> CreateComment(AbstractCommentable thing, string commentText)
        {
            string commentUrl = "api/comment";
            string data = "thing_id=t3_6q7512&text=asda";
            var response = await Post(commentUrl, new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded"));
            string tet = await response.Content.ReadAsStringAsync();
            return (HttpStatusCode.OK, tet);
        }

        public Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit)
        {
            throw new NotImplementedException();
        }

        public Task<(HttpStatusCode, string)> CreatePostAsync(Post post)
        {
            throw new NotImplementedException();
        }

        public async Task RefreshToken()
        {
            HttpResponseMessage resp1 = await Post("https://www.reddit.com/api/v1/access_token", 
                new StringContent("grant_type=refresh_token&refresh_token=51999737725-OYI8KJ5T56KSO4xAyvoVhA8t5TM", Encoding.UTF8, "application/x-www-form-urlencoded"));

            JToken responseBody = JToken.Parse(await resp1.Content.ReadAsStringAsync());
            var errors = responseBody["error"];
            var a = "";
            if (errors == null)
            {
                token = responseBody["access_token"].ToObject<string>();
            }


            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://www.reddit.com/");
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "ZXBoeHhHUjdaQTc3bkE6");
            //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            //    request.Content = content;
            //    HttpResponseMessage repsonse = await client.SendAsync(request);
            //    return repsonse;
            //}
            return;
        }



        //public async Task<string> TmpPost(string url, string data)
        //{
        //    var request2 = (HttpWebRequest)WebRequest.Create(url);

        //    request2.Method = "POST";
        //    request2.ContentType = "application/x-www-form-urlencoded";
        //    using (StreamWriter SW = new StreamWriter(request2.GetRequestStream()))
        //    {
        //        SW.Write(data);
        //    }
        //    WebResponse response = await request2.GetResponseAsync();

        //    string responseString = "";
        //    using (StreamReader SR = new StreamReader(response.GetResponseStream()))
        //    {
        //        responseString += SR.ReadLine();
        //    }
        //    return responseString;
        //}
    }
}