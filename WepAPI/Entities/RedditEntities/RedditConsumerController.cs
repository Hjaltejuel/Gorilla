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
        private const string SubscribedSubredditsUrl = "subreddits/mine/subscriber";

        private const int limit = 10;

        private const string Client_id = "ephxxGR7ZA77nA";
        private bool IsAuthenticated = false;
        private string token = "";
        private string refresh_token = "";
        //"51999737725-OYI8KJ5T56KSO4xAyvoVhA8t5TM";

        public async Task Authenticate(string code)
        {
            var request = CreateRequest(AccessTokenUrl, "POST");
            var BasicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(Client_id + ":"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", BasicAuth);
            request.Content = new StringContent($"grant_type=authorization_code&code={code}&redirect_uri=https://gorillaapi.azurewebsites.net/",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");
            var responseBody = await SendRequest(request);
            if (responseBody["error"] == null)
            {
                token = responseBody["access_token"].ToObject<string>();
                refresh_token = responseBody["refresh_token"].ToObject<string>();
            }
        }
        private HttpRequestMessage CreateRequest(string stringUri, string method)
        {
            Uri uri;
            var IsOAuth = false;
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
            if (request == null) throw new ArgumentException("No requests specified");
            //Send request
            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);
                var stringResponse = await response.Content.ReadAsStringAsync();
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
                    try
                    {
                        subreddit.posts.Add(child.data.ToObject<Post>());
                    } catch(Exception e) {
                        var a = e;
                        var b = "";
                    }
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
        public async Task<ObservableCollection<Comment>> GetMoreComments(string parentPostID, string[] children, int maxCommentsAmount=10)
        {
            var childrenString = string.Join(",", children);
            var moreChildrenUrl = $"/api/morechildren.json?api_type=json&link_id={parentPostID}&sort=hot&children={childrenString}&depth=20";
            var request = CreateRequest(moreChildrenUrl, "GET");
            var response = await SendRequest(request);
            if (response == null)
            {
                return null;
            }
            List<Comment> l = new List<Comment>();
            l.AddRange(response["json"]["data"]["things"].Select(child => child["data"].ToObject<Comment>()));
            ObservableCollection<Comment> o = new ObservableCollection<Comment>(l);
            return o;
        }

        public async Task<ObservableCollection<Post>> GetHomePageContent()
        {
            var request = CreateRequest("/", "GET");
            var response = await SendRequest(request);
            if (response == null) return null;
            if (response["error"] != null) return null;

            Listing l = response.ToObject<Listing>();
            var list = new List<Post>();
            list.AddRange(l.data.children.Select(child => child.data.ToObject<Post>()));
            return new ObservableCollection<Post>(list);
        }
        public async Task<ObservableCollection<Post>> GetUserPosts(string user)
        {
            string url = $"https://reddit.com/user/{user}/submitted/";
            var request = CreateRequest(url, "GET");
            var response = await SendRequest(request);
            if (response == null)
            {
                return null;
            }
            return CreateUserInfoCollection<Post>(response);
        }

        public async Task<ObservableCollection<Comment>> GetUserComments(string user)
        {
            string url = $"https://reddit.com/user/{user}/comments/";
            var request = CreateRequest(url, "GET");
            var response = await SendRequest(request);
            if (response == null)
            {
                return null;
            }
            return CreateUserInfoCollection<Comment>(response);


        }
        public ObservableCollection<AbstractableComment> CreateUserInfoCollection<AbstractableComment>(JToken response)
        {
            List<AbstractableComment> list = new List<AbstractableComment>();

            var listing = response.ToObject<Listing>();
            if (listing != null)
            {
                list.AddRange(
                listing.data.children.Select(child => child.data.ToObject<AbstractableComment>()));
            }

            ObservableCollection<AbstractableComment> o = new ObservableCollection<AbstractableComment>(list);


            return o;
        }
    }
}