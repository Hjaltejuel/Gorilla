﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.GorillaEntities;

namespace UI.Lib.Model.GorillaRestInterfaces
{
    public interface IRestSubredditRepository : IDisposable
    {
        Task<Subreddit> FindAsync(string subredditName);
        Task<string> CreateAsync(Subreddit subreddit);
        Task<bool> DeleteAsync(string subredditName);
        Task<IReadOnlyCollection<string>> GetLikeAsync(string like);
        Task<IReadOnlyCollection<Subreddit>> ReadAsync();

    }
}
