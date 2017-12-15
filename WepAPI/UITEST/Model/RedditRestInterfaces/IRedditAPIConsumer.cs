using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UITEST.Authentication;

namespace UITEST.RedditInterfaces
{
    public interface IRedditAPIConsumer
    {
        Task<Post> GetPostAndCommentsByIdAsync(string name_id);
        Task<Subreddit> GetSubredditAsync(string subredditName, string sortBy = "hot");

        void Authenticate(RedditAuthHandler code);
        Task<List<Subreddit>> GetSubscribedSubredditsAsync();
        Task<(HttpStatusCode, string)> CreateCommentAsync(AbstractCommentable thing, string commentText);
        Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit, bool IsSubscribing);
        Task<(HttpStatusCode, string)> CreatePostAsync(Subreddit ToSubreddit, string title, string kind, string text = "", string url = "");
        Task<(HttpStatusCode, string)> VoteAsync(AbstractCommentable commentable, int direction);
        Task<ObservableCollection<Comment>> GetMoreComments(string parentPostID, string[] children, int maxCommentsAmount = 10);
        Task<ObservableCollection<Post>> GetHomePageContent();
        Task<ObservableCollection<Post>> GetUserPosts(string user);
        
        Task<ObservableCollection<Comment>> GetUserComments(string user);
        Task<User> GetAccountDetailsAsync();

    }
}
