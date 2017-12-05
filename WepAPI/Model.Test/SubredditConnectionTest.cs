using Entities;
using Exceptions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Model.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Model.Test
{
    public class SubredditConnectionTest
    {
        [Fact]
        public async Task Create_given_SubredditConnection_adds_it()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();
                var subredditConnection = new SubredditConnection()
                {
                    SubredditFromName = "name",
                    SubredditToName = "TestSub"
                };
                var Subreddit = new Subreddit()
                {
                    SubredditName = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Subreddits.Add(Subreddit);
                context.Subreddits.Add(subredit);

                await context.SaveChangesAsync();



                using (var repository = new SubredditConnectionRepository(context))
                {
                    var db = context.SubredditConnections;
                    await repository.Create(subredditConnection);
                    Assert.Equal(subredditConnection, context.SubredditConnections.FirstOrDefault());

                }

            }

        }



        [Fact]
        public async Task Create_given_already_existing_SubredditConnection_throws_AlreadyThereException()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();
                var subredditConnection = new SubredditConnection()
                {
                    SubredditFromName = "name",
                    SubredditToName = "TestSub"
                };
                var Subreddit = new Subreddit()
                {
                    SubredditName = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Subreddits.Add(Subreddit);
                context.Subreddits.Add(subredit);
                context.SubredditConnections.Add(subredditConnection);
                await context.SaveChangesAsync();

                using (var repository = new SubredditConnectionRepository(context))
                {
                    await Assert.ThrowsAsync<AlreadyThereException>(() => repository.Create(subredditConnection));

                }

            }
        }
        [Fact]
        public async Task Create_given_non_existing_User_throws_NotFoundException()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var subredditConnection = new SubredditConnection()
                {
                    SubredditFromName = "hello",
                    SubredditToName = "TestSub"
                };
                var Subreddit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };

                context.Subreddits.Add(Subreddit);
                await context.SaveChangesAsync();

                using (var repository = new SubredditConnectionRepository(context))
                {
                    await Assert.ThrowsAsync<NotFoundException>(() => repository.Create(subredditConnection));

                }

            }
        }

        [Fact]
        public async Task Create_given_non_existing_Subreddit_throws_NotFoundException()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var subredditConnection = new SubredditConnection()
                {
                    SubredditFromName = "hello",
                    SubredditToName = "TestSub"
                };
                var Subreddit = new Subreddit()
                {
                    SubredditName = "hello"
                };

                context.Subreddits.Add(Subreddit);
                await context.SaveChangesAsync();
                using (var repository = new SubredditConnectionRepository(context))
                {
                    await Assert.ThrowsAsync<NotFoundException>(() => repository.Create(subredditConnection));

                }

            }
        }
       

        [Fact]
        public async Task Find_given_non_existing_SubredditFromName_returns_null()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                using (var repository = new SubredditConnectionRepository(context))
                {
                    var subredditConnection = await repository.Find("asdasdsadsadsadsadsadsadsadsada");

                    Assert.Null(subredditConnection);
                }
            }
        }
        [Fact]
        public async Task Create_given_non_existing_SubredditConnection_returns_Keys()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();
                var subredditConnection = new SubredditConnection()
                {
                    SubredditFromName = "name",
                    SubredditToName = "TestSub"
                };
                var Subreddit = new Subreddit()
                {
                    SubredditName = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Subreddits.Add(Subreddit);
                context.Subreddits.Add(subredit);

                await context.SaveChangesAsync();



                using (var repository = new SubredditConnectionRepository(context))
                {

                    Assert.Equal((subredditConnection.SubredditFromName, subredditConnection.SubredditToName), await repository.Create(subredditConnection));

                }

            }
        }

        [Fact]
        public async Task Get_given_non_existing_Subredditit_returns_null()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                using (var repository = new SubredditConnectionRepository(context))
                {
                    var subredditConnection = await repository.Get("asdasdsadsadsadsadsadsadsadsada","asdas");

                    Assert.Null(subredditConnection);
                }
            }
        }

        [Fact]
        public async Task Get_given_existing_Subreddit_returns_mapped_SubredditConnections()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var entity = new SubredditConnection()
                {
                    SubredditFromName = "name",
                    SubredditToName = "TestSub"

                };
         
                var user1 = new Subreddit()
                {
                    SubredditName = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Subreddits.Add(user1);
                context.Subreddits.Add(subredit);
     
                context.SubredditConnections.Add(entity);
     
                context.SaveChangesAsync();


                var SubredditFromName = entity.SubredditFromName;

                using (var repository = new SubredditConnectionRepository(context))
                {
                    var subredditConnection = await repository.Get(entity.SubredditFromName,entity.SubredditToName);

                    Assert.Equal(entity, subredditConnection);

                }
            }
        }

        [Fact]
        public async Task Read_returns_mapped_SubredditConnections()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var builder = new DbContextOptionsBuilder<RedditDBContext>()
                              .UseSqlite(connection);

            var context = new RedditDBContext(builder.Options);
            context.Database.EnsureCreated();

            var entity = new SubredditConnection
            {
                SubredditFromName = "name",
                SubredditToName = "Test"

            };
            var subredditFrom = new Subreddit
            {
                SubredditName = "name"
            };
            var subredditTo = new Subreddit
            {
                SubredditName = "Test"
            };
            context.Subreddits.Add(subredditFrom);
            context.Subreddits.Add(subredditTo);
            context.SubredditConnections.Add(entity);
            await context.SaveChangesAsync();
            var FromName = entity.SubredditFromName;
            var ToName = entity.SubredditToName;

            using (var repository = new SubredditConnectionRepository(context))
            {
                var subredditConnections = await repository.Read();
                var subredditConnection = subredditConnections.First();
                Assert.Equal(FromName, subredditConnection.SubredditFromName);
                Assert.Equal(ToName, subredditConnection.SubredditToName);

            }
        }



        [Fact]
        public async Task Update_given_existing_SubredditConnection_returns_true()
        {
            var context = new Mock<IRedditDBContext>();
            var entity = new SubredditConnection { SubredditFromName = "name", SubredditToName = "TestSub" };
            context.Setup(c => c.SubredditConnections.FindAsync("name", "TestSub")).ReturnsAsync(entity);

            using (var repository = new SubredditConnectionRepository(context.Object))
            {
                var subredditConnection = new SubredditConnection { SubredditFromName = "name", SubredditToName = "TestSub" };

                var success = await repository.Update(subredditConnection);

                Assert.True(success);
            }
        }

        [Fact]
        public async Task Update_given_non_existing_SubredditConnection_returns_false()
        {
            var context = new Mock<IRedditDBContext>();
            context.Setup(c => c.SubredditConnections.FindAsync("name", "TestSub")).ReturnsAsync(default(SubredditConnection));

            using (var repository = new SubredditConnectionRepository(context.Object))
            {
                var subredditConnection = new SubredditConnection { SubredditFromName = "name", SubredditToName = "TestSub" };

                var success = await repository.Update(subredditConnection);

                Assert.False(success);
            }
        }

        [Fact]
        public async Task Update_given_existing_SubredditConnection_Updates_properties()
        {
            var context = new Mock<IRedditDBContext>();
            var entity = new SubredditConnection { SubredditFromName = "name", SubredditToName = "TestSub", PPMI = 55, Count = 0};
            context.Setup(c => c.SubredditConnections.FindAsync("name", "TestSub")).ReturnsAsync(entity);

            using (var repository = new SubredditConnectionRepository(context.Object))
            {
                var subredditConnection = new SubredditConnection
                {
                    SubredditFromName = "name",
                    SubredditToName = "TestSub",
                    Count = 1,
                    PPMI = 5

                };

                await repository.Update(subredditConnection);
            }

            Assert.Equal(5, entity.PPMI);

        }

        [Fact]
        public async Task Update_given_existing_SubredditConnection_calls_SaveChangesAsync()
        {
            var context = new Mock<IRedditDBContext>();
            var entity = new SubredditConnection { SubredditFromName = "name", SubredditToName = "TestSub" };
            context.Setup(c => c.SubredditConnections.FindAsync("name", "TestSub")).ReturnsAsync(entity);

            using (var repository = new SubredditConnectionRepository(context.Object))
            {
                var subredditConnection = new SubredditConnection { SubredditFromName = "name", SubredditToName = "TestSub" };

                await repository.Update(subredditConnection);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact]
        public async Task Update_given_non_existing_SubredditConnection_does_not_call_SaveChangesAsync()
        {
            var context = new Mock<IRedditDBContext>();
            context.Setup(c => c.SubredditConnections.FindAsync("name", "TestSub")).ReturnsAsync(default(SubredditConnection));

            using (var repository = new SubredditConnectionRepository(context.Object))
            {
                var subredditConnection = new SubredditConnection
                {
                    SubredditFromName = "name",
                    SubredditToName = "TestSub"
                };

                await repository.Update(subredditConnection);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)), Times.Never);
        }


        [Fact]
        public async Task Delete_given_existing_keys_removes_it()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var entity = new SubredditConnection()
                {
                    SubredditFromName = "name",
                    SubredditToName = "TestSub"
                };
                var Subreddit = new Subreddit()
                {
                    SubredditName = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Add(subredit);
                context.Add(Subreddit);
                context.SubredditConnections.Add(entity);

                await context.SaveChangesAsync();

                using (var repository = new SubredditConnectionRepository(context))
                {
                    await repository.Delete("name", "TestSub");
                    Assert.Equal(0,context.SubredditConnections.Count());

                }


            }
        }



        [Fact]
        public async Task Delete_given_existing_keys_returns_true()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var entity = new SubredditConnection()
                {
                    SubredditFromName = "name",
                    SubredditToName = "TestSub"
                };
                var Subreddit = new Subreddit()
                {
                    SubredditName = "name"
                };
                var subredit = new Subreddit()
                {
                    SubredditName = "TestSub"
                };
                context.Add(subredit);
                context.Add(Subreddit);
                context.SubredditConnections.Add(entity);


                await context.SaveChangesAsync();

                using (var repository = new SubredditConnectionRepository(context))
                {

                    Assert.True(await repository.Delete("name", "TestSub"));

                }


            }
        }


        [Fact]
        public async Task Delete_given_non_existing_keys_does_not_remove_it()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();


                using (var repository = new SubredditConnectionRepository(context))
                {

                    Assert.False(await repository.Delete("name", "TestSub"));

                }


            }
        }



        [Fact]
        public void Dispose_disposes_context()
        {
            var context = new Mock<IRedditDBContext>();

            using (var repository = new SubredditConnectionRepository(context.Object))
            {
            }

            context.Verify(c => c.Dispose());
        }
    }
}
