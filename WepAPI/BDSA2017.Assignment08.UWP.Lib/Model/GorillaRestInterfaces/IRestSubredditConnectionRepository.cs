using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace UI.Lib.Model.GorillaRestInterfaces
{
    public interface IRestSubredditConnectionRepository : IDisposable
    {
        Task<SubredditConnection> GetAsync(string subredditFromName, string subredditToName);
        Task<IReadOnlyCollection<SubredditConnection>> FindAsync(string subredditFromName);
        Task<string> CreateAsync(SubredditConnection connection);
        Task<IReadOnlyCollection<SubredditConnection>> GetAllPrefs(string[] subredditFromNames);
        Task<bool> DeleteAsync(string subredditFromName, string subredditToName);
        Task<IReadOnlyCollection<SubredditConnection>> ReadAsync();
        Task<bool> UpdateAsync(SubredditConnection user);
    }
}
