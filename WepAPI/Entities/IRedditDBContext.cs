using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Entities
{
    public interface IRedditDBContext : IDisposable
    {
        DbSet<CategorySubreddit> CategorySubreddits { get; set; }
        DbSet<SubredditConnection> SubredditConnections { get; set; }
        DbSet<Subreddit> Subreddits { get; set; }
        DbSet<UserPreference> UserPreferences { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Post> Posts { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}