using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using Entities.RedditEntities;
using UITEST.Authentication;

namespace UITEST.Model.RedditRestInterfaces
{
    public interface IRedditApiConsumer
    {
        void Authenticate(RedditAuthHandler code);
        Task<(HttpStatusCode, ObservableCollection<Post>)> GetPostsByIdAsync(string things);
        Task<(HttpStatusCode, Post)> GetPostAndCommentsByIdAsync(string nameId);
        Task<(HttpStatusCode, Subreddit)> GetSubredditAsync(string subredditName);
        Task<(HttpStatusCode, Subreddit)> GetSubredditPostsAsync(Subreddit subreddit, string sortBy = "hot");
        Task<(HttpStatusCode, List<Subreddit>)> GetSubscribedSubredditsAsync();
        Task<(HttpStatusCode, ObservableCollection<Post>)> GetHomePageContent();
        Task<(HttpStatusCode, ObservableCollection<Comment>)> GetMoreComments(string parentPostId, string[] children, int depth, int maxCommentsAmount = 10);
        Task<(HttpStatusCode, ObservableCollection<Post>)> GetUserPosts(string user);
        Task<(HttpStatusCode, ObservableCollection<Comment>)> GetUserComments(string user);
        Task<(HttpStatusCode, User)> GetAccountDetailsAsync();
        Task<(HttpStatusCode, Comment)> CreateCommentAsync(AbstractCommentable thing, string commentText);
        Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit, bool isSubscribing);
        Task<(HttpStatusCode, string)> CreatePostAsync(Subreddit toSubreddit, string title, string kind, string text = "", string url = "");
        Task<(HttpStatusCode, string)> VoteAsync(AbstractCommentable commentable, int direction);
    }
}
