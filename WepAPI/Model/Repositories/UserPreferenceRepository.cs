using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaAPI.Interfaces;
using Entities.GorillaEntities;
using Microsoft.EntityFrameworkCore;

namespace Model.Repositories
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        private readonly IRedditDbContext _context;

        public UserPreferenceRepository(IRedditDbContext context)
        {
            _context = context;
            
        }

        public async Task<(string,string)> CreateAsync(UserPreference userPreference)
        {
          
            
            User user =await _context.Users.FindAsync(userPreference.Username);
            Subreddit subreddit = await _context.Subreddits.FindAsync(userPreference.SubredditName);
            var pref = await (from a in _context.UserPreferences
                              where a.Username.Equals(userPreference.Username) && a.SubredditName.Equals(userPreference.SubredditName)
                              select a).FirstOrDefaultAsync();
        
            if (user != null && subreddit != null)
            {
                if ( pref != null)
                {
                    throw new AlreadyThereException("There is already a userPreference connected");
                } 
                _context.UserPreferences.Add(userPreference);
                await _context.SaveChangesAsync();
                return (userPreference.Username, userPreference.SubredditName);
            }
            throw new NotFoundException("There is no subreddit or user to that userPreference");    

        }

        public async Task<bool> DeleteAsync(string username, string subredditName)
        {
            var preference = (from a in _context.UserPreferences.AsParallel()
                              where a.Username.Equals(username) && a.SubredditName.Equals(subredditName)
                              select a).FirstOrDefault();
            if (preference!=null)
            {
                _context.UserPreferences.Remove(preference);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<IReadOnlyCollection<UserPreference>> FindAsync(string username)
        {
           
            var prefs = await (from a in _context.UserPreferences
                         where a.Username.Equals(username)
                         select a).ToListAsync();
            
            if (!prefs.Any())
            {
                return null;
            }    
                       
            return prefs;
     
 
        }

        public async Task<bool> UpdateAsync(UserPreference userPreference)
        {
            await _context.Users.FindAsync(userPreference.Username);
            await _context.Subreddits.FindAsync(userPreference.SubredditName);
            var preference = await (from a in _context.UserPreferences
                              where a.Username.Equals(userPreference.Username) && a.SubredditName.Equals(userPreference.SubredditName)
                              select a).FirstOrDefaultAsync();
            if (preference != null)
            {
                preference.PriorityMultiplier += userPreference.PriorityMultiplier;
                await _context.SaveChangesAsync();
                return true;
            }
            else {
                if (userPreference.PriorityMultiplier < 0)
                {
                    userPreference.PriorityMultiplier = 0;
                }
                await CreateAsync(userPreference);

                return true;
            }
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
