using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaAPI.Interfaces;
using Entities.GorillaEntities;
using Microsoft.EntityFrameworkCore;

namespace Model.Repositories
{
    public class SubredditRepository : ISubredditRepository
    {
        private readonly IRedditDbContext _context;

        public SubredditRepository(IRedditDbContext context)
        {
            _context = context;
        }

        public async Task<Subreddit> FindAsync(string subredditName)
        {
            return await _context.Subreddits.FindAsync(subredditName);
            
        }



        public async Task<string> CreateAsync(Subreddit subreddit)
        {
            if((await FindAsync(subreddit.SubredditName)) != null)
            {
                throw new AlreadyThereException("A subreddit with that name already exist");
            }
            _context.Subreddits.Add(subreddit);
            await _context.SaveChangesAsync();
            return subreddit.SubredditName;
        }
        public async Task<bool> DeleteAsync(string subredditName)
        {
            var subreddit = await _context.Subreddits.FindAsync(subredditName);

            if (subreddit == null)
            {
                return false;
            }

            _context.Subreddits.Remove(subreddit);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IReadOnlyCollection<Subreddit>> ReadAsync()
        {
            return await (from s in _context.Subreddits
                   select s).ToListAsync();
        }

        public async Task<IReadOnlyCollection<Subreddit>> GetLikeAsync(string like)
        {
            return await (from s in _context.Subreddits
                          .Where(a => a.SubredditName.StartsWith($"{like}"))
                          select s).ToListAsync();
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
