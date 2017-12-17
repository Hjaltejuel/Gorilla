using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaAPI.Interfaces;
using Entities.GorillaEntities;

namespace Model.Repositories
{
    public class SubredditConnectionRepository : ISubredditConnectionRepository
    {
        private readonly IRedditDbContext _context;

        public SubredditConnectionRepository(IRedditDbContext context)
        {
            _context = context;

        }

        public async Task<(string, string)> CreateAsync(SubredditConnection subredditConnection)
        {
            Subreddit subredditFrom= await _context.Subreddits.FindAsync(subredditConnection.SubredditFromName);
            Subreddit subredditTo = await _context.Subreddits.FindAsync(subredditConnection.SubredditToName);
            var pref = await (from a in _context.SubredditConnections
                              where a.SubredditFromName.Equals(subredditConnection.SubredditFromName) && a.SubredditToName.Equals(subredditConnection.SubredditToName)
                              select a).FirstOrDefaultAsync();

            if (subredditFrom != null && subredditTo != null)
            {
                if (pref != null)
                {
                    throw new AlreadyThereException("There is already a subredditConnection connected");
                }
                _context.SubredditConnections.Add(subredditConnection);
                await _context.SaveChangesAsync();
                return (subredditConnection.SubredditFromName, subredditConnection.SubredditToName);
            }
            throw new NotFoundException("There is no subreddit or user to that subredditConnection");

        }

        public async Task<bool> DeleteAsync(string subredditFromName, string subredditToName)
        {
            var preference = (from a in _context.SubredditConnections.AsParallel()
                              where a.SubredditFromName.Equals(subredditFromName) && a.SubredditToName.Equals(subredditToName)
                              select a).FirstOrDefault();
            if (preference != null)
            {
                _context.SubredditConnections.Remove(preference);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<SubredditConnection> GetAsync(string subredditFromName, string subredditToName)
        {
            var connection = await (from a in _context.SubredditConnections
                                   where a.SubredditFromName.Equals(subredditFromName) && a.SubredditToName.Equals(subredditToName)
                                   select a).FirstOrDefaultAsync();

            return connection;
        }

        public async Task<IReadOnlyCollection<SubredditConnection>> ReadAsync()
        {
            return await (from u in _context.SubredditConnections
                          select u).ToListAsync();
        }

        public async Task<IReadOnlyCollection<SubredditConnection>> GetAllPrefs(string[] subredditFromNames)
        {
            var test = 15 / subredditFromNames.Length;
            var prefs = (from a in _context.SubredditConnections
                         where subredditFromNames.Contains(a.SubredditFromName)
                         select a).GroupBy(a => a.SubredditFromName).SelectMany(a => a.OrderByDescending(k => Decimal.Parse(k.Similarity)).Take(test)).ToList();

            return prefs;

        }
        public async Task<IReadOnlyCollection<SubredditConnection>> FindAsync(string subredditFromName)
        {

            var prefs =     await ( (from a in _context.SubredditConnections
                               where a.SubredditFromName.Equals(subredditFromName)
                               select a).OrderByDescending(a => a.Similarity)).ToListAsync();

            if (!prefs.Any())
            {
                return null;
            }

            return prefs;


        }

        public async Task<bool> UpdateAsync(SubredditConnection subredditConnection)
        {
            SubredditConnection connection = await _context.SubredditConnections.FindAsync(subredditConnection.SubredditFromName, subredditConnection.SubredditToName);
            if (connection != null)
            {
                connection.Similarity = subredditConnection.Similarity;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
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
