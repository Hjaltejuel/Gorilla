using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace UITEST.Model.GorillaRestInterfaces
{
    public interface IRestSubredditRepository : IDisposable
    {
        Task<Subreddit> FindAsync(string subredditName);
        Task<string> CreateAsync(Subreddit subreddit);
        Task<bool> DeleteAsync(string subredditName);
        Task<IReadOnlyCollection<Subreddit>> ReadAsync();

    }
}
