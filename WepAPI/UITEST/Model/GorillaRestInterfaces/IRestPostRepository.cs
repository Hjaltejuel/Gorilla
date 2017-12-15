using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorilla.Model.GorillaRestInterfaces
{
    public interface IRestPostRepository
    {
        Task<string> CreateAsync(Post post);
      
        Task<IReadOnlyCollection<Post>> ReadAsync(string username);
    }
}
