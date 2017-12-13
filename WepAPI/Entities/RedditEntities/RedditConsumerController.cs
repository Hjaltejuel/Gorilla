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

        private const string Client_id = "ephxxGR7ZA77nA";

        private string token = "";
        private string refresh_token = "51999737725-OYI8KJ5T56KSO4xAyvoVhA8t5TM";

        private HttpRequestMessage CreateRequest(string stringUri, string method)
        {
            Uri uri;
            bool IsOAuth = false;
            if (!stringUri.StartsWith("https"))
            {
                uri = new Uri(BaseUrl + stringUri + ".json?json_raw=1");
                IsOAuth = true;
            }
            else
                uri = new Uri(stringUri + ".json?json_raw=1");

            var request = new HttpRequestMessage()
            {
                RequestUri = uri                
            };
            if (IsOAuth)
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

            request.Method = new HttpMethod(method);
            //request.Headers.UserAgent.ParseAdd(UserAgent);
            request.Headers.Add("User-Agent", "Gorilla");
            return request;
        }
        public async Task<JToken> SendRequest(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentException("No requests specified");
            //Send request
            string stringResponse;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);
                stringResponse = await response.Content.ReadAsStringAsync();
                var s = token;
                JToken json;
                if (stringResponse.Length > 0)
                {
                    json = JToken.Parse(stringResponse);
                }
                else
                {
                    json = JToken.Parse($"{{'error':{response.StatusCode}}}");
                }

                return json;
            }
        }
        public HttpRequestMessage BuildRequestBody(HttpRequestMessage request, object data)
        {
            return request;
        }
        
        //CONVENTION: IF SORTBY STRING IS EMPTY, HOT IS DEFAULTED TO
        public async Task<Subreddit> GetSubredditAsync(string subredditName, string sortBy = "hot")
        {
            string aboutUri = $"https://www.reddit.com/r/{subredditName}/about";
            HttpRequestMessage aboutRequest = CreateRequest(aboutUri, "GET");
            JToken aboutResponse = await SendRequest(aboutRequest);
            if (aboutResponse["error"]==null)
            {
                ChildNode listings = aboutResponse.ToObject<ChildNode>();
                Subreddit subreddit = listings.data.ToObject<Subreddit>();

                string postsUri = $"https://www.reddit.com/r/{subredditName}/{sortBy}";
                HttpRequestMessage postsRequest = CreateRequest(postsUri, "GET");
                JToken postsResponse = await SendRequest(postsRequest);

                Listing postsListing = postsResponse.ToObject<Listing>();
                subreddit.posts = new ObservableCollection<Post>();
                foreach (ChildNode child in postsListing.data.children)
                {
                    subreddit.posts.Add(child.data.ToObject<Post>());
                }

                return subreddit;
            }
            else
            {
                return null;
            }
        }

        //t3 = comments on link / post & t5 = subreddit & t1 = comment
        public async Task<Post> GetPostAndCommentsByIdAsync(string post_id)
        {
            string uri = $"https://www.reddit.com/comments/{post_id}.json?json_raw=1";

            HttpRequestMessage request = CreateRequest(uri, "GET");
            JToken response = await SendRequest(request);
            if (response != null)
            {
                Listing[] listings = response.ToObject<Listing[]>();
                Post post = listings[0].data.children[0].data.ToObject<Post>();
                post.BuildReplies(listings[1]);
                return post;
            }
            else
                return null;
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

            return (0, null);
        }

        public async Task<User> GetAccountDetailsAsync()
        {
            HttpRequestMessage request = CreateRequest(MeUrl, "GET");
            JToken response = await SendRequest(request);
            if (response["error"] == null)
            {
                User user = response.ToObject<User>();
                return user;
            }
            else
                return null;
        }

        public async Task<(HttpStatusCode, string)> PostVoteAsync(AbstractCommentable thing, int direction)
        {
            string data = $"id={thing.name}&dir={direction}";
            HttpRequestMessage request = CreateRequest(VoteUrl, "POST");
            request.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
            JToken response = await SendRequest(request);
            if (response["error"] == null)
                return (HttpStatusCode.OK, "Vote succesful!");
            else
                return (HttpStatusCode.BadRequest, "Could not vote");
        }

        public async Task<(HttpStatusCode, string)> PostCommentAsync(AbstractCommentable thing, string commentText)
        {
            string data = $"thing_id={thing.name}&text={commentText}";
            HttpRequestMessage request = CreateRequest(CommentUrl, "POST");
            request.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
            JToken response = await SendRequest(request);
            if (response["error"] == null)
            {
                bool success = response["success"].ToObject<bool>();
                if (success)
                    return (HttpStatusCode.OK, "Comment was posted!");
                else
                    return (HttpStatusCode.BadRequest, "Could not post comment");
            }
            else
                return (HttpStatusCode.BadRequest, "Could not post comment");

        }
        public async Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit, bool IsSubscribing)
        {
            string action = IsSubscribing ? "sub" : "unsub";
            string data = $"action={action}&sr={subreddit.display_name}";
            HttpRequestMessage request = CreateRequest(CreatePostUrl, "POST");
            request.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
            JToken response = await SendRequest(request);

            if (response["error"] == null)
                return (HttpStatusCode.OK, "Subcribe successful!");
            else
                return (HttpStatusCode.BadRequest, $"Could not subscribe");
        }

        public async Task<bool> RefreshTokenAsync()
        {
            HttpRequestMessage request = CreateRequest(AccessTokenUrl, "POST");
            var BasicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(Client_id + ":"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", BasicAuth);
            request.Content = new StringContent($"grant_type=refresh_token&refresh_token={refresh_token}", Encoding.UTF8, "application/x-www-form-urlencoded");
            JToken responseBody = await SendRequest(request);
            if (responseBody["error"] == null)
            {
                token = responseBody["access_token"].ToObject<string>();
                return true;
            }
            return false;
        }

        public async Task<List<Subreddit>> GetSubscribedSubredditsAsync()
        {
            List<Subreddit> subscribedSubreddits = new List<Subreddit>();

            HttpRequestMessage request = CreateRequest(SubscribedSubredditsUrl, "GET");
            JToken response = await SendRequest(request);
            if (response["error"] == null)
            {
                Listing responseListing = response.ToObject<Listing>();
                if (responseListing == null)
                {
                    return null;
                }
                foreach (ChildNode child in responseListing.data.children)
                {
                    subscribedSubreddits.Add(child.data.ToObject<Subreddit>());
                }
                return subscribedSubreddits;
            }
            else
                return null;
        }

        public async Task<(HttpStatusCode, string)> CreatePostAsync(string title, string kind, Subreddit ToSubreddit, string text = "", string url = "")
        {
            string data = $"sr={ToSubreddit.name}&kind={kind}&title={title}&text={text}";
            HttpRequestMessage request = CreateRequest(CreatePostUrl, "POST");
            request.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
            JToken response = await SendRequest(request);

            if (response["error"] == null)
            {
                bool success = response["success"].ToObject<bool>();
                if (success)
                    return (HttpStatusCode.OK, "Post was created!");
            }
            return (HttpStatusCode.BadRequest, "Could not create post");
        }
    }
}