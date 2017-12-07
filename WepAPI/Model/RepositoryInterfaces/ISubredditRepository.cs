using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public interface ISubredditRepository : IDisposable
    {
        Task<Subreddit> FindAsync(string subredditName);
        Task<string> CreateAsync(Subreddit subreddit);
        Task<bool> DeleteAsync(string subredditName);
        Task<IReadOnlyCollection<Subreddit>> ReadAsync();

    }
}
