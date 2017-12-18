using Entities.GorillaAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaEntities;

namespace Model.Repositories
{
    class CategoryRepository : ICategoryRepository
    {
        private readonly IRedditDbContext _context;



        public async Task<bool> GetAsync(string Username, string CategoryName)
        {
            var user = _context.Users.FindAsync(Username);
            List<CategorySubreddit> listOfCategories = await (from a in _context.CategorySubreddits where a.Name == CategoryName select a).ToListAsync();

            foreach (CategorySubreddit c in listOfCategories)
            {
                if(CategoryName == c.Name)
                {
                    _context.UserPreferences.Update(new UserPreference {Username = Username, SubredditName = CategoryName, PriorityMultiplier = 1 });
                }
            }
            return true;
           
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
