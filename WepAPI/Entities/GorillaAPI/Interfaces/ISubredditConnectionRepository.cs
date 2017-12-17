using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace Entities.GorillaAPI.Interfaces
{
    public interface ISubredditConnectionRepository : IDisposable
    {
        Task<SubredditConnection> GetAsync(string subredditFromName, string subredditToName);
        Task<IReadOnlyCollection<SubredditConnection>> FindAsync(string subredditFromName);
        Task<IReadOnlyCollection<SubredditConnection>> GetAllPrefs(string[] subredditFromNames);
        Task<(string,string)> CreateAsync(SubredditConnection connection);
        Task<bool> DeleteAsync(string subredditFromName, string subredditToName);
        Task<IReadOnlyCollection<SubredditConnection>> ReadAsync();
        Task<bool> UpdateAsync(SubredditConnection user);
    }
}
