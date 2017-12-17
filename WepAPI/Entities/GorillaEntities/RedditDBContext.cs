using Microsoft.EntityFrameworkCore;

namespace Entities.GorillaEntities
{
    public sealed class RedditDbContext: DbContext, IRedditDbContext
    {
        public DbSet<CategorySubreddit> CategorySubreddits { get; set; }
        public DbSet<SubredditConnection> SubredditConnections { get; set; }
        public DbSet<Subreddit> Subreddits { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public RedditDbContext(DbContextOptions<RedditDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategorySubreddit>().HasKey(c => new { c.Name });


            modelBuilder.Entity<SubredditConnection>().HasKey(c => new { c.SubredditFromName, c.SubredditToName });


            modelBuilder.Entity<Post>().HasKey(c => new { c.username, c.Id });

            modelBuilder.Entity<UserPreference>().HasKey(c => new { c.SubredditName, c.Username});
         
           

           




        }

    }

}

