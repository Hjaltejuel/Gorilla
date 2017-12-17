using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaAPI.Interfaces;
using Entities.GorillaEntities;
using Microsoft.EntityFrameworkCore;

namespace Model.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IRedditDbContext _context;

        public UserRepository(IRedditDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateAsync(User user) 
        {
            if ((await FindAsync(user.Username)) != null)
            {
                throw new AlreadyThereException("A user witht that username alreay exist");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Username;
        }

        public async Task<bool> DeleteAsync(string username)
        {
            var user = await _context.Users.FindAsync(username);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            User userTest = await _context.Users.FindAsync(user.Username);
            if (userTest != null)
            {
                userTest.PathToProfilePicture = user.PathToProfilePicture;
                userTest.StartUpQuestionAnswered = user.StartUpQuestionAnswered;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User> FindAsync(string username)
        {
            return await _context.Users.FindAsync(username);
        }

        public async Task<IReadOnlyCollection<User>> ReadAsync()
        {
            return await (from u in _context.Users
                    select u).ToListAsync();
        }

        private bool _disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
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
