using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class UserRepository : IUserRepository
    {
        private readonly IRedditDBContext context;

        public UserRepository(IRedditDBContext _context)
        {
            context = _context;
        }
        public async Task<string> Create(User user) 
        {
            if ((await Find(user.Username)) != null)
            {
                throw new AlreadyThereException("A user witht that username alreay exist");
            }
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user.Username;
        }

        public async Task<bool> Delete(string username)
        {
            var User = await context.Users.FindAsync(username);

            if (User == null)
            {
                return false;
            }

            context.Users.Remove(User);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(User user)
        {
            User userTest = await context.Users.FindAsync(user.Username);
            if (userTest != null)
            {
                userTest.PathToProfilePicture = user.PathToProfilePicture;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User> Find(string username)
        {
            return await context.Users.FindAsync(username);
        }

        public async Task<IReadOnlyCollection<User>> Read()
        {
            return await (from u in context.Users
                    select u).ToListAsync();
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~TrackRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }
}
