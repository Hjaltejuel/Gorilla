using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace UITEST.Model.GorillaRestInterfaces
{
    public interface IRestUserPreferenceRepository: IDisposable
    {
        Task<IReadOnlyCollection<UserPreference>> FindAsync(string username);
        Task<string> CreateAsync(UserPreference userPreference);
        Task<bool> DeleteAsync(string username, string subredditName);
        Task<bool> UpdateAsync(UserPreference userPreference);
    }
}
