using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public interface ISubredditRepository : IDisposable
    {
        Task<Subreddit> Find(string subredditName);
        Task<string> Create(Subreddit subreddit);
        Task<bool> Delete(string subredditName);
        Task<IReadOnlyCollection<Subreddit>> Read();

    }
}
