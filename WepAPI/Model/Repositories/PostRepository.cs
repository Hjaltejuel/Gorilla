using Entities.GorillaAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Exceptions;
using Entities.GorillaEntities;

namespace Model.Repositories
{
    public class PostRepository : IPostRepository
    {

        private readonly IRedditDbContext _context;
 
        public PostRepository(IRedditDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateAsync(Post post)
        {
            var postTest = await (from a in _context.Posts
                              where a.username.Equals(post.username) && a.Id.Equals(post.Id)
                              select a).FirstOrDefaultAsync();
                              
            if (postTest != null)
            {
                throw new AlreadyThereException("");
            }

            if ((await ReadAsync(post.username)).Count > 5)
            {
               
                Post remove = await (from a in _context.Posts
                                 where a.username.Equals(post.username)
                                 select a).FirstOrDefaultAsync();
                _context.Posts.Remove(remove);
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

          
            return post.Id;
        }

        public virtual async Task<IReadOnlyCollection<Post>> ReadAsync(string username)
        {
            return await (from s in _context.Posts
                          where username.Equals(s.username) 
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
