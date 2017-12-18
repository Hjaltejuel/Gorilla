using Entities.GorillaAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaEntities;

namespace Model.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IRedditDbContext _context;
        private readonly IUserPreferenceRepository _repository;
        
        public CategoryRepository(IRedditDbContext context, IUserPreferenceRepository repository)
        {
            _repository = repository;
            _context = context;
        }


        public async Task<bool> UpdateAsync(string Username, string[] CategoryNames)
        {
            var user = await _context.Users.FindAsync(Username);
            List<string> listOfCategories = await (from a in _context.CategorySubreddits
                                                   where CategoryNames.Contains(a.Name)
                                                   select a.SubredditName).ToListAsync();

            foreach (string c in listOfCategories)
            {
                    await _repository.UpdateAsync(new UserPreference {Username = Username, SubredditName = c, PriorityMultiplier = 1 });
                
            }
            await _context.SaveChangesAsync();
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
