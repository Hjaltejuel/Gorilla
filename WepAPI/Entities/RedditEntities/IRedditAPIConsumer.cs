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
        Task<Subreddit> GetSubredditAsync(string subredditName, string sortBy="hot");
        Task<(HttpStatusCode, string)> LoginToReddit(string username, string password);
        Task<(HttpStatusCode, string)> CreateComment(AbstractCommentable thing, string commentText);
        Task<(HttpStatusCode, string)> SubscribeToSubreddit(Subreddit subreddit);
        Task<(HttpStatusCode, string)> CreatePostAsync(Post post);
        Task<(HttpStatusCode, string)> PostVoteAsync(AbstractCommentable commentable, int direction);
        Task<User> GetAccountDetails();
        Task RefreshToken();

    }
}
