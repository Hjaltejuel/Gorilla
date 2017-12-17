﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Entities.GorillaAPI.Interfaces
{
     public interface ICategoryRepository : IDisposable
    {
        Task<bool> GetAsync(string Username, string CategoryName);
    }
}
