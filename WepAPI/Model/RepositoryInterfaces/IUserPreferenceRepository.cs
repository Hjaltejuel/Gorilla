﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IUserPreferenceRepository: IDisposable
    {
        Task<IReadOnlyCollection<UserPreference>> FindAsync(string username);
        Task<(string,string)> CreateAsync(UserPreference userPreference);
        Task<bool> DeleteAsync(string username, string subredditName);
        Task<bool> UpdateAsync(UserPreference userPreference);
    }
}
