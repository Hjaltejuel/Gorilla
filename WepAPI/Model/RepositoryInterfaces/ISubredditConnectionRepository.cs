using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface ISubredditConnectionRepository : IDisposable
    {
        Task<SubredditConnection> Get(string subredditFromName, string subredditToName);
        Task<IReadOnlyCollection<SubredditConnection>> Find(string subredditFromName);
        Task<(string,string)> Create(SubredditConnection connection);
        Task<bool> Delete(string subredditFromName, string subredditToName);
        Task<IReadOnlyCollection<SubredditConnection>> Read();
        Task<bool> Update(SubredditConnection user);
    }
}
