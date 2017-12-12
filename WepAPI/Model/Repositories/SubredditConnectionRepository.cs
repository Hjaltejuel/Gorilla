using Entities;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Repositories
{
   public class SubredditConnectionRepository : ISubredditConnectionRepository
    {
        private readonly IRedditDBContext context;

        public SubredditConnectionRepository(IRedditDBContext _context)
        {
            context = _context;

        }

        public async Task<(string, string)> CreateAsync(SubredditConnection subredditConnection)
        {


            Subreddit subredditFrom= await context.Subreddits.FindAsync(subredditConnection.SubredditFromName);
            Subreddit subredditTo = await context.Subreddits.FindAsync(subredditConnection.SubredditToName);
            var pref = await (from a in context.SubredditConnections
                              where a.SubredditFromName.Equals(subredditConnection.SubredditFromName) && a.SubredditToName.Equals(subredditConnection.SubredditToName)
                              select a).FirstOrDefaultAsync();

            if (subredditFrom != null && subredditTo != null)
            {
                if (pref != null)
                {
                    throw new AlreadyThereException("There is already a subredditConnection connected");
                }
                context.SubredditConnections.Add(subredditConnection);
                await context.SaveChangesAsync();
                return (subredditConnection.SubredditFromName, subredditConnection.SubredditToName);
            }
            throw new NotFoundException("There is no subreddit or user to that subredditConnection");

        }

        public async Task<bool> DeleteAsync(string SubredditFromName, string SubredditToName)
        {
            var preference = (from a in context.SubredditConnections.AsParallel()
                              where a.SubredditFromName.Equals(SubredditFromName) && a.SubredditToName.Equals(SubredditToName)
                              select a).FirstOrDefault();
            if (preference != null)
            {
                context.SubredditConnections.Remove(preference);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<SubredditConnection> GetAsync(string subredditFromName, string subredditToName)
        {
            var connection = await (from a in context.SubredditConnections
                                   where a.SubredditFromName.Equals(subredditFromName) && a.SubredditToName.Equals(subredditToName)
                                   select a).FirstOrDefaultAsync();

            if(connection == null)
            {
                return null;
            } else{ return connection; };
                                  
        }

        public async Task<IReadOnlyCollection<SubredditConnection>> ReadAsync()
        {
            return await (from u in context.SubredditConnections
                          select u).ToListAsync();
        }

        public async Task<IReadOnlyCollection<SubredditConnection>> FindAsync(string SubredditFromName)
        {

            var prefs =  await (from a in context.SubredditConnections
                               where a.SubredditFromName.Equals(SubredditFromName)
                               select a).ToListAsync();

            if (prefs.Count() == 0)
            {
                return null;
            }

            return prefs;


        }

        public async Task<bool> UpdateAsync(SubredditConnection subredditConnection)
        {
            SubredditConnection connection = await context.SubredditConnections.FindAsync(subredditConnection.SubredditFromName, subredditConnection.SubredditToName);
            if (connection != null)
            {
                connection.Count = subredditConnection.Count;
                connection.PPMI = subredditConnection.PPMI;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
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
