using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IUserPreferenceRepository: IDisposable
    {
        Task<IReadOnlyCollection<UserPreference>> FindAll(string username);
        Task<(string,string)> Create(UserPreference userPreference);
        Task<bool> Delete(string username, string subredditName);
        Task<bool> Update(UserPreference userPreference);
    }
}
