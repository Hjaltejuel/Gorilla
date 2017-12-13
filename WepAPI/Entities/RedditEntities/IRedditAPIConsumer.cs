using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Entities.RedditEntities
{
    public interface IRedditAPIConsumer
    {
        Task<Post> GetPostAndCommentsByIdAsync(string name_id);
        Task<Subreddit> GetSubredditAsync(string subredditName, string sortBy = "hot");
        Task<List<Subreddit>> GetSubscribedSubredditsAsync();
        Task<(HttpStatusCode, string)> LoginToReddit(string username, string password);
        Task<(HttpStatusCode, string)> PostCommentAsync(AbstractCommentable thing, string commentText);
        Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit, bool IsSubscribing);
        Task<(HttpStatusCode, string)> CreatePostAsync(string title, string kind, Subreddit ToSubreddit, string text = "", string url = "");
        Task<(HttpStatusCode, string)> PostVoteAsync(AbstractCommentable commentable, int direction);
        Task<User> GetAccountDetailsAsync();
        Task<bool> RefreshTokenAsync();

    }
}
