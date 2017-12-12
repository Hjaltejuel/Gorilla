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

        Task<User> GetAccountDetails(); 

        Task<(HttpStatusCode, string)> PostPostAsync(Post p);

        Task<(HttpStatusCode, string)> PostCommentAsync(Comment_old c);

        Task<(HttpStatusCode, string)> PostVoteAsync(Vote v);

    }
}
