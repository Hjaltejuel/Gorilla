using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IUserRepository: IDisposable
    {
        Task<User> Find(string username);
        Task<String> Create(User user);
        Task<bool> Delete(string username);
        Task<IReadOnlyCollection<User>> Read();
        Task<bool> Update(User user);
    }
}
