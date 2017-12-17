using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace Entities.GorillaAPI.Interfaces
{
    public interface IPostRepository : IDisposable
    {
        Task<string> CreateAsync(Post post);
        Task<IReadOnlyCollection<Post>> ReadAsync(string username);

    }
}
