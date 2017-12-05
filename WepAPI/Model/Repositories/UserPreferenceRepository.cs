using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Exceptions;

namespace Model
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        private readonly IRedditDBContext context;

        public UserPreferenceRepository(IRedditDBContext _context)
        {
            context = _context;
            
        }

        public async Task<(string,string)> Create(UserPreference userPreference)
        {
          
            
            User user =await context.Users.FindAsync(userPreference.Username);
            Subreddit subreddit = await context.Subreddits.FindAsync(userPreference.SubredditName);
            var pref = await (from a in context.UserPreferences
                              where a.Username.Equals(userPreference.Username) && a.SubredditName.Equals(userPreference.SubredditName)
                              select a).FirstOrDefaultAsync();
        
            if (user != null && subreddit != null)
            {
                if ( pref != null)
                {
                    throw new AlreadyThereException("There is already a userPreference connected");
                } 
                context.UserPreferences.Add(userPreference);
                await context.SaveChangesAsync();
                return (userPreference.Username, userPreference.SubredditName);
            }
            throw new NotFoundException("There is no subreddit or user to that userPreference");    

        }

        public async Task<bool> Delete(string username, string subredditName)
        {
            var preference = (from a in context.UserPreferences.AsParallel()
                              where a.Username.Equals(username) && a.SubredditName.Equals(subredditName)
                              select a).FirstOrDefault();
            if (preference!=null)
            {
                context.UserPreferences.Remove(preference);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<IReadOnlyCollection<UserPreference>> FindAll(string username)
        {
           
            var prefs = await (from a in context.UserPreferences
                         where a.Username.Equals(username)
                         select a).ToListAsync();
            
            if (prefs.Count() == 0)
            {
                return null;
            }    
                       
            return prefs;
     
 
        }

        public async Task<bool> Update(UserPreference userPreference)
        {
            UserPreference preference = await context.UserPreferences.FindAsync(userPreference.Username, userPreference.SubredditName);
            if (preference != null)
            {
                preference.PriorityMultiplier = userPreference.PriorityMultiplier;
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
