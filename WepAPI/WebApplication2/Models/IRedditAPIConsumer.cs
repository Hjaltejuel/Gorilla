using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    interface IRedditAPIConsumer : IWebConsumer
    {
        Task<Post> GetPostAsync(string name_id);

        Task<Subreddit> GetSubredditAsync(string subredditName, string sortBy);

        Task<(HttpStatusCode, string)> LoginToReddit(string username, string password);

        Task<(HttpStatusCode, string)> PostPostAsync(Post p);

        Task<(HttpStatusCode, string)> PostCommentAsync(Comment c);

        Task<(HttpStatusCode, string)> PostVoteAsync(Vote v);
    }
}
