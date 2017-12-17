using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace UI.Lib.Model.GorillaRestInterfaces
{
    public interface IRestPostRepository
    {
        Task<string> CreateAsync(Post post);
      
        Task<IReadOnlyCollection<Post>> ReadAsync(string username);
    }
}
