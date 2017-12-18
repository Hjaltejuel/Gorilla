using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace UI.Lib.Model.GorillaRestInterfaces
{
    public interface IRestUserRepository: IDisposable
    {
        Task<User> FindAsync(string username);
        Task<String> CreateAsync(User user);
        Task<bool> DeleteAsync(string username);
        Task<IReadOnlyCollection<User>> ReadAsync();
        Task<bool> UpdateAsync(User user);
  
    }
}
