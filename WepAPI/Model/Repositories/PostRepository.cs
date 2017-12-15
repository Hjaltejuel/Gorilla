using Entities;
using Entities.GorillaAPI.Interfaces;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Repositories
{
    public class PostRepository : IPostRepository
    {

        private readonly IRedditDBContext context;

        public PostRepository(IRedditDBContext _context)
        {
            context = _context;
        }
        public async Task<string> CreateAsync(Post post)
        {
            if (context.Posts.Find(post.Id) != null)
            {
                throw new AlreadyThereException("");
            }

            if ((await ReadAsync(post.username)).Count() > 5)
            {
               
                Post remove = await (from a in context.Posts
                                 where a.username.Equals(post.username)
                                 select a).FirstOrDefaultAsync();
                context.Posts.Remove(remove);
            }

            context.Posts.Add(post);
            await context.SaveChangesAsync();

          
            return post.Id;
        }

        public async Task<IReadOnlyCollection<Post>> ReadAsync(string username)
        {
            return await (from s in context.Posts
                          where username.Equals(s.username) 
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
