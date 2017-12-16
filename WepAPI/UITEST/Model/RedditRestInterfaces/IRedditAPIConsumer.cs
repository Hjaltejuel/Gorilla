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
        Task<Post> GetPostAndCommentsByIdAsync(string nameId);
        Task<Subreddit> GetSubredditAsync(string subredditName, string sortBy = "hot");

        void Authenticate(RedditAuthHandler code);
        Task<List<Subreddit>> GetSubscribedSubredditsAsync();
        Task<(HttpStatusCode, string)> CreateCommentAsync(AbstractCommentable thing, string commentText);
        Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit, bool isSubscribing);
        Task<(HttpStatusCode, string)> CreatePostAsync(Subreddit toSubreddit, string title, string kind, string text = "", string url = "");
        Task<(HttpStatusCode, string)> VoteAsync(AbstractCommentable commentable, int direction);
        Task<ObservableCollection<Post>> GetHomePageContent();
        Task<ObservableCollection<Comment>> GetMoreComments(string parentPostId, string[] children, int depth, int maxCommentsAmount = 10);
        Task<ObservableCollection<Post>> GetUserPosts(string user);
        
        Task<ObservableCollection<Comment>> GetUserComments(string user);
        Task<User> GetAccountDetailsAsync();
        List<Comment> BuildCommentList(List<Comment> commentList, int depth);
    }
}
