using Entities.RedditEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Entities.RedditEntities
{
    public interface IRedditAPIConsumer : IWebConsumer
    {
        Task<Post> GetPostAndCommentsByIdAsync(string name_id);
        Task<Subreddit> GetSubredditAsync(string subredditName, string sortBy = "hot");
        Task<List<Subreddit>> GetSubscribedSubreddits();
        Task<(HttpStatusCode, string)> LoginToReddit(string username, string password);
        Task<(HttpStatusCode, string)> PostComment(AbstractCommentable thing, string commentText);
        Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit, bool IsSubscribing);
        Task<(HttpStatusCode, string)> CreatePostAsync(string subreddit, string title, string kind, string url, string text, Subreddit ToSubreddit);
        Task<(HttpStatusCode, string)> PostVoteAsync(AbstractCommentable commentable, int direction);
        Task<User> GetAccountDetails();
        Task RefreshToken();

    }
}
