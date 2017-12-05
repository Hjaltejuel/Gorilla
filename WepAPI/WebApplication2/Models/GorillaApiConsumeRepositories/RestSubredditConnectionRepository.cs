using Entities;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApplication2.Models.GorillaApiConsumeRepositories
{
    public class RestSubredditConnectionRepository: ISubredditConnectionRepository
    {
        private readonly Uri _baseAddress = new Uri("http://gorillaapi.azurewebsites.net/");

        private readonly HttpClient _client;

        public RestSubredditConnectionRepository(HttpClient client)
        {
            client.BaseAddress = _baseAddress;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _client = client;
        }

        public async Task<(string,string)> CreateAsync(SubredditConnection subredditConnection)
        {
            var response = await _client.PostAsync("api/subredditConnection", subredditConnection.ToHttpContent());

            if (response.IsSuccessStatusCode)
            {
                var location = response.Headers.GetValues("Location").First();
                return (location,null);
            }

            return (null,null);
        }

        public Task<IReadOnlyCollection<SubredditConnection>> FindAsync(string subredditFromName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string subredditFromName, string subredditToName)
        {
            var response = await _client.DeleteAsync($"api/subredditConnection/{subredditFromName}{subredditToName}");

            return response.IsSuccessStatusCode;
        }

        public async Task<SubredditConnection> GetAsync(string subredditFromName, string subredditToName)
        {
            var response = await _client.GetAsync($"api/subredditConnection/{subredditFromName}{subredditToName}");

            if (response.IsSuccessStatusCode)
            {
                var subredditConnection = await response.Content.To<SubredditConnection>();

                return subredditConnection;
            }

            return null;
        }

        public async Task<IReadOnlyCollection<SubredditConnection>> ReadAsync()
        {
            var response = await _client.GetAsync("api/subredditConnection");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.To<IReadOnlyCollection<SubredditConnection>>();
            }

            return null;
        }

        public async Task<bool> UpdateAsync(SubredditConnection subredditConnection)
        {
            var response = await _client.PutAsync($"api/subredditConnection", subredditConnection.ToHttpContent());

            return response.IsSuccessStatusCode;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CharacterRepository() {
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

      

       
        #endregion
    }
}
