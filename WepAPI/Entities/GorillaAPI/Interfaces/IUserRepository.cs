using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace Entities.GorillaAPI.Interfaces
{
    public interface IUserRepository: IDisposable
    {
        Task<User> FindAsync(string username);
        Task<String> CreateAsync(User user);
        Task<bool> DeleteAsync(string username);
        Task<IReadOnlyCollection<User>> ReadAsync();
        Task<bool> UpdateAsync(User user);
        
    }
}
