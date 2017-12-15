using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Entities.GorillaAPI.Interfaces
{
    public interface IPostRepository : IDisposable
    {
        Task<string> CreateAsync(Post post);
        Task<IReadOnlyCollection<Post>> ReadAsync(string username);

    }
}
