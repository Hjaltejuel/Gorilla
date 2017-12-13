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
using System.Linq;
using System.Windows;

namespace Entities.RedditEntities    
{
    public class RedditConsumerController : IRedditAPIConsumer
    {
        
        public string BaseUrl => "https://oauth.reddit.com/";

        private const string CommentUrl = "api/comment";
        private const string MeUrl = "api/v1/me";
        private const string AccessTokenUrl = "https://www.reddit.com/api/v1/access_token";
        private const string VoteUrl = "api/vote";
        private const string CreatePostUrl = "api/submit";
        private const string SubscribeUrl = "api/subscribe";
        private const string SubscribedSubredditsUrl = "subreddits/mine";

        private const string ClientId = "ephxxGR7ZA77nA";

        private string _token = "";
        private const string refresh_token = "51999737725-OYI8KJ5T56KSO4xAyvoVhA8t5TM";

        public RedditConsumerController()
        {
            RefreshTokenAsync();
        }
        private HttpRequestMessage CreateRequest(string stringUri, string method)
        {
            Uri uri;
            var isOAuth = false;
            if (!stringUri.StartsWith("https"))
            {
                uri = new Uri(BaseUrl + stringUri + ".json?json_raw=1");
                isOAuth = true;
            }
            else
                uri = new Uri(stringUri + ".json?json_raw=1");

            var request = new HttpRequestMessage()
            {
                RequestUri = uri                
            };
            if (isOAuth)
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);

            request.Method = new HttpMethod(method);
            //request.Headers.UserAgent.ParseAdd(UserAgent);
            request.Headers.Add("User-Agent", "Gorilla");
            return request;
        }
        public async Task<JToken> SendRequest(HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentException("No requests specified");
            //Send request
            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);
                var stringResponse = await response.Content.ReadAsStringAsync();
                var s = _token;
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
            var aboutUri = $"https://www.reddit.com/r/{subredditName}/about";
            var aboutRequest = CreateRequest(aboutUri, "GET");
            var aboutResponse = await SendRequest(aboutRequest);
            if (aboutResponse["error"]==null)
            {
                var listings = aboutResponse.ToObject<ChildNode>();
                var subreddit = listings.data.ToObject<Subreddit>();

                var postsUri = $"https://www.reddit.com/r/{subredditName}/{sortBy}";
                var postsRequest = CreateRequest(postsUri, "GET");
                var postsResponse = await SendRequest(postsRequest);

                var postsListing = postsResponse.ToObject<Listing>();
                subreddit.posts = new ObservableCollection<Post>();
                foreach (var child in postsListing.data.children)
                {
                    subreddit.posts.Add(child.data.ToObject<Post>());
                }
                return subreddit;
            }
            return null;
        }
        //t3 = comments on link / post & t5 = subreddit & t1 = comment
        public async Task<Post> GetPostAndCommentsByIdAsync(string post_id)
        {
            var uri = $"https://www.reddit.com/comments/{post_id}.json?json_raw=1";

            var request = CreateRequest(uri, "GET");
            var response = await SendRequest(request);
            if (response == null) return null;

            var listings = response.ToObject<Listing[]>();
            var post = listings[0].data.children[0].data.ToObject<Post>();
            post.BuildReplies(listings[1]);
            return post;
        }
        public async Task<(HttpStatusCode, string)> ResponseStatusAsync(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var statusCode = response.StatusCode;
            return (statusCode, responseBody);
        }
        public async Task<(HttpStatusCode, string)> LoginToReddit(string username, string password)
        {
            /*
            "access_token": "daAVJCA7aA38doAt5_FcS6wR-2U",
            "token_type": "bearer",
            "expires_in": 3600,
            "refreshToken": "51999737725-OYI8KJ5T56KSO4xAyvoVhA8t5TM",
            "scope": "*"
            */
            return (0, null);
        }
        public async Task<User> GetAccountDetailsAsync()
        {
            var request = CreateRequest(MeUrl, "GET");
            var response = await SendRequest(request);
            if (response["error"] != null) return null;
            var user = response.ToObject<User>();
            return user;
        }
        //Direction: -1 downvote | 0 remove vote | 1 upvote
        public async Task<(HttpStatusCode, string)> VoteAsync(AbstractCommentable thing, int direction)
        {
            var data = $"id={thing.name}&dir={direction}";
            var request = CreateRequest(VoteUrl, "POST");
            request.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await SendRequest(request);
            if (response["error"] == null)
                return (HttpStatusCode.OK, "Vote succesful!");
            else
                return (HttpStatusCode.BadRequest, "Could not vote");
        }
        public async Task<(HttpStatusCode, string)> CreateCommentAsync(AbstractCommentable thing, string commentText)
        {
            var data = $"thing_id={thing.name}&text={commentText}";
            var request = CreateRequest(CommentUrl, "POST");
            request.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await SendRequest(request);

            if (response["error"] != null) return (HttpStatusCode.BadRequest, "Could not post comment");

            var success = response["success"].ToObject<bool>();
            if (success)
                return (HttpStatusCode.OK, "Comment was posted!");
            return (HttpStatusCode.BadRequest, "Could not post comment");
        }
        public async Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit, bool IsSubscribing)
        {
            var action = IsSubscribing ? "sub" : "unsub";
            var data = $"action={action}&sr={subreddit.name}";
            var request = CreateRequest(SubscribeUrl, "POST");
            request.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await SendRequest(request);
            
            if (response["error"] != null) return (HttpStatusCode.BadRequest, "Could not subscribe");

            return (HttpStatusCode.OK, "Subcribe successful!");
        }
        public async Task<bool> RefreshTokenAsync()
        {
            var request = CreateRequest(AccessTokenUrl, "POST");
            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientId + ":"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            request.Content = new StringContent($"grant_type=refresh_token&refresh_token={refresh_token}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var responseBody = await SendRequest(request);

            if (responseBody["error"] != null) return false;

            _token = responseBody["access_token"].ToObject<string>();
            return true;
        }
        public async Task<List<Subreddit>> GetSubscribedSubredditsAsync()
        {
            var subscribedSubreddits = new List<Subreddit>();

            var request = CreateRequest(SubscribedSubredditsUrl, "GET");
            var response = await SendRequest(request);
            if (response["error"] != null) return null;

            var responseListing = response.ToObject<Listing>();
            if (responseListing == null) return null;

            subscribedSubreddits.AddRange(
                responseListing.data.children.Select(child => child.data.ToObject<Subreddit>()));
            return subscribedSubreddits;
        }
        //Kind: self | link
        // if kind is link, then url must be specified
        public async Task<(HttpStatusCode, string)> CreatePostAsync(Subreddit toSubreddit, string title, string kind, string text = "", string url = "")
        {
            string data;
            if (kind.Equals("link"))
                data = $"sr={toSubreddit.display_name}&kind={kind}&title={title}&url={url}";
            else
                data = $"sr={toSubreddit.display_name}&kind={kind}&title={title}&text={text}";

            var request = CreateRequest(CreatePostUrl, "POST");
            request.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await SendRequest(request);
            if (response["error"] != null) return (HttpStatusCode.BadRequest, "Could not create post");
            var success = response["success"].ToObject<bool>();

            if (success)
                return (HttpStatusCode.OK, "Post was created!");
            return (HttpStatusCode.BadRequest, "Could not create post");
        }
        public async Task<List<AbstractCommentable>> GetMoreComments(AbstractCommentable parentComment, string[] children, int maxCommentsAmount=10)
        {
            var childrenString = string.Join(",", children);
            var moreChildrenUrl = $"/api/morechildren.json?api_type=json&link_id={parentComment.name}&children={childrenString}";
            var request = CreateRequest(moreChildrenUrl, "GET");
            var response = await SendRequest(request);
            if (response["error"] == null)
            {
                return null;
            }
            return null;
        }
    }
}