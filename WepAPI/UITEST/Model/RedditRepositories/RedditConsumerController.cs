using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Entities.RedditEntities;
using Newtonsoft.Json.Linq;
using UITEST.Authentication;
using UITEST.Model.RedditRestInterfaces;

namespace UITEST.Model.RedditRepositories
{
    public class RedditConsumerController : IRedditApiConsumer
    {
        public string BaseUrl => "https://oauth.reddit.com/";
        private const string CommentUrl = "api/comment";
        private const string MeUrl = "api/v1/me";
        private const string VoteUrl = "api/vote";
        private const string CreatePostUrl = "api/submit";
        private const string SubscribeUrl = "api/subscribe";
        private const string SubscribedSubredditsUrl = "subreddits/mine/subscriber";
        private const string GetByIdUrl = "by_id/{0}";
        RedditAuthHandler _authHandler;

        public void Authenticate(RedditAuthHandler handler)
        {
            _authHandler = handler;
        }

        private HttpRequestMessage CreateRequest(string stringUri, string method, string data = "")
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
                request = _authHandler.AuthenticateRequest(request);

            request.Method = new HttpMethod(method);
            if (!data.IsNullOrEmpty())
            {
                request.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
            }
            request.Headers.Add("User-Agent", "Gorilla");
            return request;
        }
        public async Task<(HttpStatusCode, JToken)> SendRequest(HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentException("No requests specified");
            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);
                var stringResponse = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode) return (response.StatusCode, null);
                if (stringResponse.Length == 0) return (HttpStatusCode.BadRequest, null);

                var json = JToken.Parse(stringResponse);
                try
                {
                    if (json.Value<string>("error") != null) return (HttpStatusCode.BadRequest, json["error"]);
                }
                catch
                {
                    //This is actually good - so no need to handle the catch
                }
                return (HttpStatusCode.OK, json);
            }
        }
        public async Task<(HttpStatusCode, ObservableCollection<Post>)> GetPostsByIdAsync(string things)
        {
            var request = CreateRequest(string.Format(GetByIdUrl, things), "GET");
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);

            var list = new ObservableCollection<Post>();
            foreach (var child in response.Item2.ToObject<Listing>().data.children)
            {
                list.Add(child.data.ToObject<Post>());
            }
            return (response.Item1, list);
        }

        public async Task<(HttpStatusCode, Subreddit)> GetSubredditAsync(string subredditName)
        {
            var aboutUri = $"https://www.reddit.com/r/{subredditName}/about";
            var request = CreateRequest(aboutUri, "GET");
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);

            var subreddit = response.Item2.ToObject<ChildNode>().data.ToObject<Subreddit>();
            return (response.Item1, subreddit);
        }

        //CONVENTION: IF SORTBY STRING IS EMPTY, HOT IS DEFAULTED TO
        public async Task<(HttpStatusCode, Subreddit)> GetSubredditPostsAsync(Subreddit subreddit, string sortBy = "hot")
        {
            var postsUri = $"https://www.reddit.com/r/{subreddit.display_name}/{sortBy}";
            var request = CreateRequest(postsUri, "GET");
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);

            var postsListing = response.Item2.ToObject<Listing>();
            subreddit.posts = new ObservableCollection<Post>();
            foreach (var child in postsListing.data.children)
            {
                try
                {
                    subreddit.posts.Add(child.data.ToObject<Post>());
                }
                catch
                {
                    // ignored
                }
            }
            return (response.Item1, subreddit);
        }

        //t3 = comments on link / post & t5 = subreddit & t1 = comment
        public async Task<(HttpStatusCode, Post)> GetPostAndCommentsByIdAsync(string postId)
        {
            var uri = $"https://www.reddit.com/comments/{postId}.json?json_raw=1";

            var request = CreateRequest(uri, "GET");
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);

            var listings = response.Item2.ToObject<Listing[]>();
            var post = listings[0].data.children[0].data.ToObject<Post>();
            post.BuildReplies(listings[1]);
            return (response.Item1, post);
        }
        public async Task<(HttpStatusCode, User)> GetAccountDetailsAsync()
        {
            var request = CreateRequest(MeUrl, "GET");
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);

            var user = response.Item2.ToObject<User>();
            return (response.Item1, user);
        }
        //Direction: -1 downvote | 0 remove vote | 1 upvote
        public async Task<(HttpStatusCode, string)> VoteAsync(AbstractCommentable thing, int direction)
        {
            var data = $"id={thing.name}&dir={direction}";
            var request = CreateRequest(VoteUrl, "POST", data);
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, "Could not vote");

            return (response.Item1, "Vote succesful!");
        }
        public async Task<(HttpStatusCode, Comment)> CreateCommentAsync(AbstractCommentable thing, string commentText)
        {
            var data = $"thing_id={thing.name}&text={commentText}";
            var request = CreateRequest(CommentUrl, "POST", data);
            var response = await SendRequest(request);

            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);

            var success = response.Item2["success"].ToObject<bool>();
            if (!success) return (HttpStatusCode.BadRequest, null);

            var responseComment = BuildCommentFromResponse(response.Item2["jquery"]);
            if (thing.GetType() == typeof(Comment))
            {
                responseComment.depth = ((Comment)thing).depth + 1;
            }
            return (response.Item1, responseComment);
        }

        public Comment BuildCommentFromResponse(JToken responseData)
        {
            var jArray = responseData.ToObject<JArray>();
            foreach (var token in jArray)
            {
                var lastElement = token.Last;
                if (!lastElement.HasValues) continue;
                lastElement = lastElement.First;
                if (!lastElement.HasValues) continue;

                if (lastElement.GetType() == typeof(JArray))
                {
                    return lastElement.First.ToObject<ChildNode>().data.ToObject<Comment>();
                }
            }
            return null;
        }

        public async Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit, bool isSubscribing)
        {
            var action = isSubscribing ? "sub" : "unsub";
            var data = $"action={action}&sr={subreddit.name}";
            var request = CreateRequest(SubscribeUrl, "POST", data);
            var response = await SendRequest(request);

            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);

            return (HttpStatusCode.OK, "Subcribe successful!");
        }
        public async Task<(HttpStatusCode, List<Subreddit>)> GetSubscribedSubredditsAsync()
        {
            var subscribedSubreddits = new List<Subreddit>();

            var request = CreateRequest(SubscribedSubredditsUrl, "GET");
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);

            var responseListing = response.Item2.ToObject<Listing>();

            subscribedSubreddits.AddRange(responseListing.data.children.Select(child => child.data.ToObject<Subreddit>()));

            return (response.Item1, subscribedSubreddits);
        }
        //Kind: self | link
        // if kind is link, then url must be specified
        public async Task<(HttpStatusCode, string)> CreatePostAsync(Subreddit toSubreddit, string title, string kind, string text = "", string url = "")
        {
            var data = $"sr={toSubreddit.display_name}&kind={kind}&title={title}";
            if (kind.Equals("link"))
                data += $"&url={url}";
            else
                data = $"&text={text}";

            var request = CreateRequest(CreatePostUrl, "POST", data);
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, "Could not create post");

            var success = response.Item2["success"].ToObject<bool>();
            if (success) return (response.Item1, "Post was created!");

            return (HttpStatusCode.BadRequest, "Could not create post");
        }

        public async Task<(HttpStatusCode, ObservableCollection<Comment>)> GetMoreComments(string parentPostId, string[] children, int depth, int maxCommentsAmount = 10)
        {
            var childrenString = string.Join(",", children);
            var moreChildrenUrl = $"/api/morechildren.json?api_type=json&link_id={parentPostId}&sort=hot&children={childrenString}&depth=20";
            var request = CreateRequest(moreChildrenUrl, "GET");
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);
            var l = new List<Comment>();
            
            l.AddRange(
            response.Item2["json"]["data"]["things"].Select(child => child["data"].ToObject<Comment>()));
            var o = new ObservableCollection<Comment>(l);
            return (response.Item1, o);
        }

        public async Task<(HttpStatusCode, ObservableCollection<Post>)> GetHomePageContent()
        {
            var request = CreateRequest("/", "GET");
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);

            var l = response.Item2.ToObject<Listing>();
            var list = new List<Post>();
            list.AddRange(l.data.children.Select(child => child.data.ToObject<Post>()));
            return (response.Item1, new ObservableCollection<Post>(list));
        }
        public async Task<(HttpStatusCode, ObservableCollection<Post>)> GetUserPosts(string user)
        {
            var url = $"https://reddit.com/user/{user}/submitted/";
            var request = CreateRequest(url, "GET");
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);
            return (response.Item1, CreateUserInfoCollection<Post>(response.Item2));
        }

        public async Task<(HttpStatusCode, ObservableCollection<Comment>)> GetUserComments(string user)
        {
            var url = $"https://reddit.com/user/{user}/comments/";
            var request = CreateRequest(url, "GET");
            var response = await SendRequest(request);
            if (response.Item1 != HttpStatusCode.OK) return (response.Item1, null);
            return (response.Item1, CreateUserInfoCollection<Comment>(response.Item2));


        }
        public ObservableCollection<AbstractableComment> CreateUserInfoCollection<AbstractableComment>(JToken response)
        {
            var list = new List<AbstractableComment>();

            var listing = response.ToObject<Listing>();
            if (listing != null)
            {
                list.AddRange(
                listing.data.children.Select(child => child.data.ToObject<AbstractableComment>()));
            }

            var o = new ObservableCollection<AbstractableComment>(list);
            return o;
        }
    }
}