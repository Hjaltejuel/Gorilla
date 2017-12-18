using Entities.GorillaAPI.Interfaces;
using Entities.GorillaEntities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Model.Test
{
    public class CategoryRepositoryTest
    {
        [Fact]
        public async void test()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);
                builder.EnableSensitiveDataLogging();

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();
                var category = new CategorySubreddit { Name = "Science", SubredditName = "AskReddit" };
                var category1 = new CategorySubreddit { Name = "Science", SubredditName = "Sven" };
                var category2 = new CategorySubreddit { Name = "ds", SubredditName = "AskReddit" };
                var category3 = new CategorySubreddit { Name = "ds", SubredditName = "AskReddit2" };


                context.CategorySubreddits.Add(category);
                context.CategorySubreddits.Add(category1);
                context.CategorySubreddits.Add(category2);
                context.CategorySubreddits.Add(category3);

                context.Users.Add(new User { Username = "Hjalte" });

                await context.SaveChangesAsync();



                using (var repository = new CategoryRepository(context, (new Mock<IUserPreferenceRepository>()).Object)) 
                {
                    await repository.UpdateAsync( "Hjalte", new string[] {"Science","ds" });
                    Assert.True(true);

                }
            }
        }
    }
}
