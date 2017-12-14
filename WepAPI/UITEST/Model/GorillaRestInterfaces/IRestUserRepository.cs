using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IRestUserRepository: IDisposable
    {
        Task<User> FindAsync(string username);
        Task<String> CreateAsync(User user);
        Task<bool> DeleteAsync(string username);
        Task<IReadOnlyCollection<User>> ReadAsync();
        Task<bool> UpdateAsync(User user);
        Task<byte[]> FindImageAsync(string username);
    }
}
