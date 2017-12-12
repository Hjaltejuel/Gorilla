using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;

namespace Entities.RedditEntities    
{
    public class RedditConsumerController : IRedditAPIConsumer
    {
        public string BaseUrl { get => "https://oauth.reddit.com/"; }

        private const string CommentUrl = "api/comment";
        private const string MeUrl = "api/v1/me";
        private const string AccessTokenUrl = "https://www.reddit.com/api/v1/access_token";
        private const string VoteUrl = "api/vote";
        private const string CreatePostUrl = "api/submit";
        private const string SubscribeUrl = "api/subscribe";
        private const string SubscribedSubredditsUrl = "subreddits/mine";


        private string token = "Ta1lmwOuJYc_wxOYSp6Oa4wgcgo";
        private string refresh_token = "51999737725-OYI8KJ5T56KSO4xAyvoVhA8t5TM";
        
        public async Task<HttpResponseMessage> Get(string uri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("User-Agent", "Gorilla");
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    await RefreshToken();
                }
                return response;
            }
        }
        


        public async Task<HttpResponseMessage> Post(string uri, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
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
            HttpResponseMessage response = await Get(MeUrl);
            User user = JToken.Parse(await response.Content.ReadAsStringAsync()).ToObject<User>();
            return user;
        }

        public async Task<(HttpStatusCode, string)> PostVoteAsync(AbstractCommentable thing, int direction)
        {
            string data = $"id={thing.name}&dir={direction}";
            var response = await Post(VoteUrl, new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded"));
            if (response.StatusCode == HttpStatusCode.OK)
                    return (HttpStatusCode.OK, "Vote succesful!");
            else
                return (response.StatusCode, "Could not vote");

        }

        public async Task<(HttpStatusCode, string)> PostComment(AbstractCommentable thing, string commentText)
        {
            string data = $"thing_id={thing.name}&text={commentText}";
            var response = await Post(CommentUrl, new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded"));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JToken responseObject = JToken.Parse(await response.Content.ReadAsStringAsync());
                bool success = responseObject["success"].ToObject<bool>();
                if (success)
                    return (HttpStatusCode.OK, "Comment was posted!");
                else
                    return (HttpStatusCode.BadRequest, "Could not post comment");
            }
            else
                return (response.StatusCode, "Could not post comment");

        }

        public async Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit, bool IsSubscribing)
        {
            string action = IsSubscribing ? "sub" : "unsub";

            string data = $"action={action}&sr={subreddit.display_name}";
            var response = await Post(CreatePostUrl, new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded"));
            if (response.StatusCode == HttpStatusCode.OK)
                return (HttpStatusCode.OK, $"{action}cribed successful!");
            else
                return (response.StatusCode, $"Could not {action}bscribe");
        }

        public async Task RefreshToken()
        {
            HttpResponseMessage resp1 = await Post(
                AccessTokenUrl,
                new StringContent($"grant_type=refresh_token&refresh_token={refresh_token}", Encoding.UTF8, "application/x-www-form-urlencoded")
                );

            JToken responseBody = JToken.Parse(await resp1.Content.ReadAsStringAsync());
            var errors = responseBody["error"];
            var a = "";
            if (errors == null)
            {
                token = responseBody["access_token"].ToObject<string>();
            }
            return;
        }

        public async Task<List<Subreddit>> GetSubscribedSubreddits()
        {
            List<Subreddit> subscribedSubreddits = new List<Subreddit>();

            HttpResponseMessage response = await Get(SubscribedSubredditsUrl);
            Listing responseListing = JToken.Parse(await response.Content.ReadAsStringAsync()).ToObject<Listing>();
            if (responseListing == null)
            {
                return null;
            }

            foreach(ChildNode child in responseListing.data.children)
            {
                subscribedSubreddits.Add(child.data.ToObject<Subreddit>());
            }

            return subscribedSubreddits;
        }

        public async Task<(HttpStatusCode, string)> CreatePostAsync(string subreddit, string title, string kind, string url, string text, Subreddit ToSubreddit)
        {
            string data = $"sr={ToSubreddit.name}&kind={kind}&title={title}&text={text}";
            var response = await Post(CreatePostUrl, new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded"));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JToken responseObject = JToken.Parse(await response.Content.ReadAsStringAsync());
                bool success = responseObject["success"].ToObject<bool>();
                if (success)
                    return (HttpStatusCode.OK, "Post was created!");
                else
                    return (HttpStatusCode.BadRequest, "Could not create post");
            }
            else
                return (response.StatusCode, "Could not create post");
        }
    }
}