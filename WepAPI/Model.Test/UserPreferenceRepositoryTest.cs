using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaEntities;
using Model.Repositories;
using Xunit;

namespace Model.Test
{
    public class UserPreferenceRepositoryTest
    {
        [Fact]
        public async Task Create_given_UserPreference_adds_it()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();
                var userPreference = new UserPreference()
                {
                    Username = "name",
                    SubredditName = "TestSub"
                };
                var user = new User()
                {
                    Username = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Users.Add(user);
                context.Subreddits.Add(subredit);

                await context.SaveChangesAsync();



                using (var repository = new UserPreferenceRepository(context))
                {
                    await repository.CreateAsync(userPreference);
                    Assert.Equal(userPreference, context.UserPreferences.FirstOrDefault());

                }

            }

        }

       

        [Fact]
        public async Task Create_given_already_existing_UserPreference_throws_AlreadyThereException()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();
                var userPreference = new UserPreference()
                {
                    Username = "name",
                    SubredditName = "TestSub"
                };
                var user = new User()
                {
                    Username = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Users.Add(user);
                context.Subreddits.Add(subredit);
                context.UserPreferences.Add(userPreference);
                await context.SaveChangesAsync();
               
                using (var repository = new UserPreferenceRepository(context))
                {
                    await Assert.ThrowsAsync<AlreadyThereException>(() => repository.CreateAsync(userPreference));

                }

            }
        }
        [Fact]
        public async Task Create_given_non_existing_User_throws_NotFoundException()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();
            
                var userPreference = new UserPreference()
                {
                    Username = "hello",
                    SubredditName = "TestSub"
                };
                var subreddit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };

                context.Subreddits.Add(subreddit);
                await context.SaveChangesAsync();

                using (var repository = new UserPreferenceRepository(context))
                {
                    await Assert.ThrowsAsync<NotFoundException>(() => repository.CreateAsync(userPreference));

                }

            }
        }

        [Fact]
        public async Task Create_given_non_existing_Subreddit_throws_NotFoundException()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var userPreference = new UserPreference()
                {
                    Username = "hello",
                    SubredditName = "TestSub"
                };
                var user = new User()
                {
                    Username = "hello"
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
                using (var repository = new UserPreferenceRepository(context))
                {
                    await Assert.ThrowsAsync<NotFoundException>(() => repository.CreateAsync(userPreference));

                }

            }
        }
        [Fact]
        public async Task Create_given_non_existing_UserPreference_returns_Keys()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();
                var userPreference = new UserPreference()
                {
                    Username = "name",
                    SubredditName = "TestSub"
                };
                var user = new User()
                {
                    Username = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Users.Add(user);
                context.Subreddits.Add(subredit);

                await context.SaveChangesAsync();



                using (var repository = new UserPreferenceRepository(context))
                {
                    
                    Assert.Equal((userPreference.Username,userPreference.SubredditName), await repository.CreateAsync(userPreference));

                }

            }
        }

        [Fact]
        public async Task FindAll_given_non_existing_username_returns_null()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                using (var repository = new UserPreferenceRepository(context))
                {
                    var userPreference =  await repository.FindAsync("asdasdsadsadsadsadsadsadsadsada");

                    Assert.Null(userPreference);
                }
            }
        }

        [Fact]
        public async Task FindAll_given_existing_username_returns_mapped_UserPreferences()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var entity = new UserPreference()
                {
                    Username = "name",
                    SubredditName = "TestSub"

                };
                var entity2 = new UserPreference()
                {
                    Username = "name",
                    SubredditName = "TestSub2"

                };
                var user1 = new User()
                {
                    Username = "name"
                };
            var subredit = new Subreddit()
            {
                SubredditName = "TestSub"
            };
            var subredit2 = new Subreddit()
            {
                SubredditName = "TestSub2"
            };
                ICollection<UserPreference> preference = new List<UserPreference>();
                context.Users.Add(user1);
                context.Subreddits.Add(subredit);
                context.Subreddits.Add(subredit2);
                context.UserPreferences.Add(entity);
                context.UserPreferences.Add(entity2);
                await context.SaveChangesAsync();

                preference.Add(entity2);
                preference.Add(entity);

                using (var repository = new UserPreferenceRepository(context))
                {
                    var userPreference = await repository.FindAsync(entity.Username);

                    Assert.Equal(preference.Count,userPreference.Count);

                }
            }
        }



        [Fact]
        public async Task Update_given_existing_UserPreference_returns_true()
        {
            var context = new Mock<IRedditDbContext>();
            var entity = new UserPreference { Username = "name", SubredditName = "TestSub" };
            context.Setup(c => c.UserPreferences.FindAsync("name","TestSub")).ReturnsAsync(entity);

            using (var repository = new UserPreferenceRepository(context.Object))
            {
                var userPreference = new UserPreference { Username = "name", SubredditName = "TestSub" };

                var success = await repository.UpdateAsync(userPreference);

                Assert.True(success);
            }
        }

        [Fact]
        public async Task Update_given_non_existing_UserPreference_returns_false()
        {
            var context = new Mock<IRedditDbContext>();
            context.Setup(c => c.UserPreferences.FindAsync("name","TestSub")).ReturnsAsync(default(UserPreference));

            using (var repository = new UserPreferenceRepository(context.Object))
            {
                var userPreference = new UserPreference { Username = "name", SubredditName = "TestSub" };

                var success = await repository.UpdateAsync(userPreference);

                Assert.False(success);
            }
        }

        [Fact]
        public async Task Update_given_existing_UserPreference_Updates_properties()
        {
            var context = new Mock<IRedditDbContext>();
            var entity = new UserPreference { Username = "name" , SubredditName = "TestSub", PriorityMultiplier = 5 };
            context.Setup(c => c.UserPreferences.FindAsync("name","TestSub")).ReturnsAsync(entity);

            using (var repository = new UserPreferenceRepository(context.Object))
            {
                var userPreference = new UserPreference
                {
                    Username = "name",
                    SubredditName = "TestSub",
                    PriorityMultiplier = 1

                };

                await repository.UpdateAsync(userPreference);
            }

            Assert.Equal(1, entity.PriorityMultiplier);

        }

        [Fact]
        public async Task Update_given_existing_UserPreference_calls_SaveChangesAsync()
        {
            var context = new Mock<IRedditDbContext>();
            var entity = new UserPreference { Username = "name" , SubredditName = "TestSub" };
            context.Setup(c => c.UserPreferences.FindAsync("name","TestSub")).ReturnsAsync(entity);

            using (var repository = new UserPreferenceRepository(context.Object))
            {
                var userPreference = new UserPreference { Username = "name", SubredditName = "TestSub" };

                await repository.UpdateAsync(userPreference);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact]
        public async Task Update_given_non_existing_UserPreference_does_not_call_SaveChangesAsync()
        {
            var context = new Mock<IRedditDbContext>();
            context.Setup(c => c.UserPreferences.FindAsync("name","TestSub")).ReturnsAsync(default(UserPreference));

            using (var repository = new UserPreferenceRepository(context.Object))
            {
                var userPreference = new UserPreference
                {
                    Username = "name" ,
                    SubredditName = "TestSub"
                };

                await repository.UpdateAsync(userPreference);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)), Times.Never);
        }


        [Fact]
        public async Task Delete_given_existing_keys_removes_it()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var entity = new UserPreference()
                {
                    Username = "name",
                    SubredditName = "TestSub"
                };
                var user = new User()
                {
                    Username = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Add(subredit);
                context.Add(user);
                context.UserPreferences.Add(entity);

                await context.SaveChangesAsync();

                using (var repository = new UserPreferenceRepository(context))
                {
                    await repository.DeleteAsync("name", "TestSub");
                    Assert.Equal(0,context.UserPreferences.Count());

                }


            }
        }

    

        [Fact]
        public async Task Delete_given_existing_keys_returns_true()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var entity = new UserPreference()
                {
                    Username = "name",
                    SubredditName = "TestSub"
                };
                var user = new User()
                {
                    Username = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Add(subredit);
                context.Add(user);
                context.UserPreferences.Add(entity);


                await context.SaveChangesAsync();

                using (var repository = new UserPreferenceRepository(context))
                {
                   
                    Assert.True(await repository.DeleteAsync("name", "TestSub"));

                }


            }
        }


        [Fact]
        public async Task Delete_given_non_existing_keys_does_not_remove_it()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();


                using (var repository = new UserPreferenceRepository(context))
                {

                    Assert.False(await repository.DeleteAsync("name", "TestSub"));

                }


            }
        }

   

        [Fact]
        public void Dispose_disposes_context()
        {
            var context = new Mock<IRedditDbContext>();

            new UserPreferenceRepository(context.Object).Dispose();

            context.Verify(c => c.Dispose());
        }
    }
}
