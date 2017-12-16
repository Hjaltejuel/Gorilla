using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace Entities.GorillaAPI.Interfaces
{
    public interface IUserPreferenceRepository: IDisposable
    {
        Task<IReadOnlyCollection<UserPreference>> FindAsync(string username);
        Task<(string,string)> CreateAsync(UserPreference userPreference);
        Task<bool> DeleteAsync(string username, string subredditName);
        Task<bool> UpdateAsync(UserPreference userPreference);
    }
}
