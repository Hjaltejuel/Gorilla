using Entities;
using Exceptions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Model.Test
{
    public class SubredditRepositoryTest
    {
        [Fact]
        public async Task Create_given_Subreddit_adds_it()
        {
            var entity = default(Subreddit);
            var context = new Mock<IRedditDBContext>();
            context.Setup(c => c.Subreddits.Add(It.IsAny<Subreddit>())).Callback<Subreddit>(t => entity = t);

            using (var repository = new SubredditRepository(context.Object))
            {
                var subreddit = new Subreddit
                {
                    SubredditName = "name",
                   
                };
                await repository.CreateAsync(subreddit);
            }

            Assert.Equal("name", entity.SubredditName);
        
        }

        [Fact]
        public async Task Create_given_Subreddit_calls_SaveChangesAsync()
        {
            var context = new Mock<IRedditDBContext>();
            context.Setup(c => c.Subreddits.Add(It.IsAny<Subreddit>()));

            using (var repository = new SubredditRepository(context.Object))
            {
                var user = new Subreddit { SubredditName = "name" };

                await repository.CreateAsync(user);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact]
        public async Task Create_given_already_existing_Subreddit_throws_AlreadyThereException()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();
                var subreddit = new Subreddit() { SubredditName = "name" };
                context.Subreddits.Add(subreddit);
                using (var repository = new SubredditRepository(context))
                {
                   await Assert.ThrowsAsync<AlreadyThereException>(() => repository.CreateAsync(subreddit));

                }

            }
        }

        [Fact]
        public async Task Create_given_non_existing_Subreddit_returns_Key()
        {
            var entity = default(Subreddit);

            var context = new Mock<IRedditDBContext>();
            context.Setup(c => c.Subreddits.Add(It.IsAny<Subreddit>()))
                .Callback<Subreddit>(t => entity = t);
            context.Setup(c => c.SaveChangesAsync(default(CancellationToken)))
                .Returns(Task.FromResult(0))
                .Callback(() => entity.SubredditName = "name");

            using (var repository = new SubredditRepository(context.Object))
            {
                var subreddit = new Subreddit { SubredditName = "name" };

                var subredditName = await repository.CreateAsync(subreddit);

                Assert.Equal("name", subredditName);
            }
        }

        [Fact]
        public async Task Find_given_non_existing_key_returns_null()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                using (var repository = new SubredditRepository(context))
                {
                    var user = await repository.FindAsync("asdasdsadsadsadsadsadsadsadsada");

                    Assert.Null(user);
                }
            }
        }

        [Fact]
        public async Task Find_given_existing_key_returns_mapped_Subreddit()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDBContext>()
                                  .UseSqlite(connection);

                var context = new RedditDBContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var entity = new Subreddit
                {
                    SubredditName = "name",
                   
                };

                context.Subreddits.Add(entity);
                await context.SaveChangesAsync();
                var SubredditName = entity.SubredditName;

                using (var repository = new SubredditRepository(context))
                {
                    var user = await repository.FindAsync(entity.SubredditName);

                    Assert.Equal("name", user.SubredditName);
           
                }
            }
        }

        [Fact]
        public async Task Read_returns_mapped_TrackDTO()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var builder = new DbContextOptionsBuilder<RedditDBContext>()
                              .UseSqlite(connection);

            var context = new RedditDBContext(builder.Options);
            context.Database.EnsureCreated();

            var entity = new Subreddit
            {
                SubredditName = "name",

            };

            context.Subreddits.Add(entity);
            await context.SaveChangesAsync();
            var SubredditName = entity.SubredditName;

            using (var repository = new SubredditRepository(context))
            {
                var Subreddits = await repository.ReadAsync();
                var subreddit = Subreddits.FirstOrDefault();
                Assert.Equal("name", subreddit.SubredditName);
            
            }
        }


        [Fact]
        public async Task Delete_given_existing_SubredditName_removes_it()
        {
            var context = new Mock<IRedditDBContext>();
            var subreddit = new Subreddit { SubredditName = "name" };
            context.Setup(c => c.Subreddits.FindAsync("name")).ReturnsAsync(subreddit);

            using (var repository = new SubredditRepository(context.Object))
            {
                await repository.DeleteAsync("name");
            }

            context.Verify(c => c.Subreddits.Remove(subreddit));
        }

        [Fact]
        public async Task Delete_given_existing_SubreditName_calls_SaveChangesAsync()
        {
            var context = new Mock<IRedditDBContext>();
            var subreddit = new Subreddit { SubredditName = "name" };
            context.Setup(c => c.Subreddits.FindAsync("name")).ReturnsAsync(subreddit);

            using (var repository = new SubredditRepository(context.Object))
            {
                await repository.DeleteAsync("name");
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact]
        public async Task Delete_given_existing_SubredditName_returns_true()
        {
            var context = new Mock<IRedditDBContext>();
            var subreddit = new Subreddit { SubredditName = "name" };
            context.Setup(c => c.Subreddits.FindAsync("name")).ReturnsAsync(subreddit);

            using (var repository = new SubredditRepository(context.Object))
            {
                var success = await repository.DeleteAsync("name");

                Assert.True(success);
            }
        }

        [Fact]
        public async Task Delete_given_non_existing_SubredditName_does_not_call_SaveChangesAsync()
        {
            var context = new Mock<IRedditDBContext>();
            context.Setup(c => c.Subreddits.FindAsync("name")).ReturnsAsync(default(Subreddit));

            using (var repository = new SubredditRepository(context.Object))
            {
                await repository.DeleteAsync("name");
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)), Times.Never);
        }

        [Fact]
        public async Task Delete_given_non_existing_SubredditName_does_not_remove_it()
        {
            var context = new Mock<IRedditDBContext>();
            context.Setup(c => c.Subreddits.FindAsync("name")).ReturnsAsync(default(Subreddit));

            using (var repository = new SubredditRepository(context.Object))
            {
                await repository.DeleteAsync("name");
            }

            context.Verify(c => c.Subreddits.Remove(It.IsAny<Subreddit>()), Times.Never);
        }

        [Fact]
        public async Task Delete_given_non_existing_SubredditName_returns_false()
        {
            var context = new Mock<IRedditDBContext>();
            context.Setup(c => c.Subreddits.FindAsync("name")).ReturnsAsync(default(Subreddit));

            using (var repository = new SubredditRepository(context.Object))
            {
                var success = await repository.DeleteAsync("name");

                Assert.False(success);
            }
        }

        [Fact]
        public void Dispose_disposes_context()
        {
            var context = new Mock<IRedditDBContext>();

            using (var repository = new SubredditRepository(context.Object))
            {
            }

            context.Verify(c => c.Dispose());
        }
    }
}
