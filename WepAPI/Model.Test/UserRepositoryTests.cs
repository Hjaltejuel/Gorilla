using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaEntities;
using Model.Repositories;
using Xunit;

namespace Model.Test
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task Create_given_User_adds_it()
        {
            var entity = default(User);
            var context = new Mock<IRedditDbContext>();
            context.Setup(c => c.Users.Add(It.IsAny<User>())).Callback<User>(t => entity = t);

            using (var repository = new UserRepository(context.Object))
            {
                var user = new User
                {
                    Username = "name",

                };
                await repository.CreateAsync(user);
            }

            Assert.Equal("name", entity.Username);

        }

        [Fact]
        public async Task Create_given_User_calls_SaveChangesAsync()
        {
            var context = new Mock<IRedditDbContext>();
            context.Setup(c => c.Users.Add(It.IsAny<User>()));

            using (var repository = new UserRepository(context.Object))
            {
                var user = new User{ Username = "name" };

                await repository.CreateAsync(user);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact]
        public async Task Create_given_already_existing_User_throws_AlreadyThereException()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();
                var user = new User() { Username = "name" };
                context.Users.Add(user);
                using (var repository = new UserRepository(context))
                {
                    await Assert.ThrowsAsync<AlreadyThereException>(() => repository.CreateAsync(user));

                }

            }
        }

        [Fact]
        public async Task Create_given_non_existing_User_returns_Key()
        {
            var entity = default(User);

            var context = new Mock<IRedditDbContext>();
            context.Setup(c => c.Users.Add(It.IsAny<User>()))
                .Callback<User>(t => entity = t);
            context.Setup(c => c.SaveChangesAsync(default(CancellationToken)))
                .Returns(Task.FromResult(0))
                .Callback(() => entity.Username = "name");

            using (var repository = new UserRepository(context.Object))
            {
                var user = new User{ Username = "name" };

                var username = await repository.CreateAsync(user);

                Assert.Equal("name", username);
            }
        }

        [Fact]
        public async Task Find_given_non_existing_key_returns_null()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                using (var repository = new UserRepository(context))
                {
                    var user = await repository.FindAsync("asdasdsadsadsadsadsadsadsadsada");

                    Assert.Null(user);
                }
            }
        }

        [Fact]
        public async Task Find_given_existing_key_returns_mapped_User()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<RedditDbContext>()
                                  .UseSqlite(connection);

                var context = new RedditDbContext(builder.Options);
                await context.Database.EnsureCreatedAsync();

                var entity = new User
                {
                    Username = "name",

                };

                context.Users.Add(entity);
                await context.SaveChangesAsync();

                using (var repository = new UserRepository(context))
                {
                    var user = await repository.FindAsync(entity.Username);

                    Assert.Equal("name", user.Username);

                }
            }
        }

        [Fact]
        public async Task Read_returns_mapped_User()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var builder = new DbContextOptionsBuilder<RedditDbContext>()
                              .UseSqlite(connection);

            var context = new RedditDbContext(builder.Options);
            context.Database.EnsureCreated();

            var entity = new User
            {
                Username = "name",

            };

            context.Users.Add(entity);
            await context.SaveChangesAsync();

            using (var repository = new UserRepository(context))
            {
                var users = await repository.ReadAsync();
                var user = users.First();
                Assert.Equal("name", user.Username);

            }
        }



        [Fact]
        public async Task Update_given_existing_User_returns_true()
        {
            var context = new Mock<IRedditDbContext>();
            var entity = new User { Username = "name" };
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(entity);

            using (var repository = new UserRepository(context.Object))
            {
                var user = new User { Username = "name" };

                var success = await repository.UpdateAsync(user);

                Assert.True(success);
            }
        }

        [Fact]
        public async Task Update_given_non_existing_User_returns_false()
        {
            var context = new Mock<IRedditDbContext>();
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(default(User));

            using (var repository = new UserRepository(context.Object))
            {
                var user = new User { Username = "name" };

                var success = await repository.UpdateAsync(user);

                Assert.False(success);
            }
        }

        [Fact]
        public async Task Update_given_existing_track_Updates_properties()
        {
            var context = new Mock<IRedditDbContext>();
            var entity = new User { Username = "name" };
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(entity);

            using (var repository = new UserRepository(context.Object))
            {
                var user = new User
                {
                    Username = "name",
                   
                };

                await repository.UpdateAsync(user);
            }

            Assert.Equal("name", entity.Username);
           
        }

        [Fact]
        public async Task Update_given_existing_User_calls_SaveChangesAsync()
        {
            var context = new Mock<IRedditDbContext>();
            var entity = new User { Username = "name" };
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(entity);

            using (var repository = new UserRepository(context.Object))
            {
                var user = new User { Username = "name" };

                await repository.UpdateAsync(user);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact]
        public async Task Update_given_non_existing_User_does_not_call_SaveChangesAsync()
        {
            var context = new Mock<IRedditDbContext>();
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(default(User));

            using (var repository = new UserRepository(context.Object))
            {
                var user = new User { Username = "name" };

                await repository.UpdateAsync(user);
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)), Times.Never);
        }


        [Fact]
        public async Task Delete_given_existing_username_removes_it()
        {
            var context = new Mock<IRedditDbContext>();
            var user = new User{ Username = "name" };
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(user);

            using (var repository = new UserRepository(context.Object))
            {
                await repository.DeleteAsync("name");
            }

            context.Verify(c => c.Users.Remove(user));
        }

        [Fact]
        public async Task Delete_given_existing_username_calls_SaveChangesAsync()
        {
            var context = new Mock<IRedditDbContext>();
            var user = new User{ Username = "name" };
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(user);

            using (var repository = new UserRepository(context.Object))
            {
                await repository.DeleteAsync("name");
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)));
        }

        [Fact]
        public async Task Delete_given_existing_username_returns_true()
        {
            var context = new Mock<IRedditDbContext>();
            var user = new User{ Username = "name" };
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(user);

            using (var repository = new UserRepository(context.Object))
            {
                var success = await repository.DeleteAsync("name");

                Assert.True(success);
            }
        }

        [Fact]
        public async Task Delete_given_non_existing_username_does_not_call_SaveChangesAsync()
        {
            var context = new Mock<IRedditDbContext>();
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(default(User));

            using (var repository = new UserRepository(context.Object))
            {
                await repository.DeleteAsync("name");
            }

            context.Verify(c => c.SaveChangesAsync(default(CancellationToken)), Times.Never);
        }

        [Fact]
        public async Task Delete_given_non_existing_username_does_not_remove_it()
        {
            var context = new Mock<IRedditDbContext>();
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(default(User));

            using (var repository = new UserRepository(context.Object))
            {
                await repository.DeleteAsync("name");
            }

            context.Verify(c => c.Users.Remove(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Delete_given_non_existing_username_returns_false()
        {
            var context = new Mock<IRedditDbContext>();
            context.Setup(c => c.Users.FindAsync("name")).ReturnsAsync(default(User));

            using (var repository = new UserRepository(context.Object))
            {
                var success = await repository.DeleteAsync("name");

                Assert.False(success);
            }
        }

        [Fact]
        public void Dispose_disposes_context()
        {
            var context = new Mock<IRedditDbContext>();
            new UserRepository(context.Object).Dispose();

            context.Verify(c => c.Dispose());
        }
    }
}
