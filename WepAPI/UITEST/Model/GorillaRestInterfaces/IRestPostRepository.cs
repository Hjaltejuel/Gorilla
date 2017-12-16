using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace UITEST.Model.GorillaRestInterfaces
{
    public interface IRestPostRepository
    {
        Task<string> CreateAsync(Post post);
      
        Task<IReadOnlyCollection<Post>> ReadAsync(string username);
    }
}
