using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace Entities.GorillaAPI.Interfaces
{
    public interface ISubredditRepository : IDisposable
    {
        Task<Subreddit> FindAsync(string subredditName);
        Task<string> CreateAsync(Subreddit subreddit);
        Task<IReadOnlyCollection<string>> GetLikeAsync(string like);
        Task<bool> DeleteAsync(string subredditName);
        Task<IReadOnlyCollection<Subreddit>> ReadAsync();

    }
}
