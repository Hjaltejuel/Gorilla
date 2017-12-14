using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IRestSubredditConnectionRepository : IDisposable
    {
        Task<SubredditConnection> GetAsync(string subredditFromName, string subredditToName);
        Task<IReadOnlyCollection<SubredditConnection>> FindAsync(string subredditFromName);
        Task<string> CreateAsync(SubredditConnection connection);
        Task<bool> DeleteAsync(string subredditFromName, string subredditToName);
        Task<IReadOnlyCollection<SubredditConnection>> ReadAsync();
        Task<bool> UpdateAsync(SubredditConnection user);
    }
}
