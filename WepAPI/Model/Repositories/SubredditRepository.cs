using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Linq;
using Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class SubredditRepository : ISubredditRepository
    {
        private readonly IRedditDBContext context;

        public SubredditRepository(IRedditDBContext _context)
        {
            context = _context;
        }

        public async Task<Subreddit> Find(string subredditName)
        {
            return await context.Subreddits.FindAsync(subredditName);
            
        }

        public async Task<string> Create(Subreddit subreddit)
        {
            if((await Find(subreddit.SubredditName)) != null)
            {
                throw new AlreadyThereException("A subreddit with that name already exist");
            }
            context.Subreddits.Add(subreddit);
            await context.SaveChangesAsync();
            return subreddit.SubredditName;
        }
        public async Task<bool> Delete(string subredditName)
        {
            var subreddit = await context.Subreddits.FindAsync(subredditName);

            if (subreddit == null)
            {
                return false;
            }

            context.Subreddits.Remove(subreddit);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<IReadOnlyCollection<Subreddit>> Read()
        {
            return await (from s in context.Subreddits
                   select s).ToListAsync();
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
