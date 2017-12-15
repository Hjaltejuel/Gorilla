using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Text;

namespace Entities
{
    public class RedditDBContext: DbContext, IRedditDBContext
    {
        public virtual DbSet<CategorySubreddit> CategorySubreddits { get; set; }
        public virtual DbSet<SubredditConnection> SubredditConnections { get; set; }
        public virtual DbSet<Subreddit> Subreddits { get; set; }
        public virtual DbSet<UserPreference> UserPreferences { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public RedditDBContext(DbContextOptions<RedditDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategorySubreddit>().HasKey(c => new { c.Name });


            modelBuilder.Entity<SubredditConnection>().HasKey(c => new { c.SubredditFromName, c.SubredditToName });
          



            modelBuilder.Entity<UserPreference>().HasKey(c => new { c.SubredditName, c.Username});
         
           

           




        }

    }

}

