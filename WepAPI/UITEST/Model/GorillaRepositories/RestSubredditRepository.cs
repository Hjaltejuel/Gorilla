using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using System.Net.Http;
using System.Net.Http.Headers;
using Gorilla.AuthenticationGorillaAPI;
using System.Net;

namespace WebApplication2.Models.GorillaApiConsumeRepositories
{
    public class RestSubredditRepository : IRestSubredditRepository
    {
        private readonly Uri _baseAddress = new Uri("https://gorillaapi.azurewebsites.net/");

        private readonly HttpClient _client;
        private readonly IAuthenticationHelper _helper;

        public RestSubredditRepository(ISettings settings, DelegatingHandler handler, IAuthenticationHelper helper)
        {
            _helper = helper;
            var client = new HttpClient(handler)
            {
                BaseAddress = settings.ApiBaseAddress
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _client = client;
        }
        public async Task<string> CreateAsync(Entities.Subreddit subreddit)
        {
            using (var h = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), new Uri("https://gorillaapi.azurewebsites.net/api/subreddit"));
                request.Content = subreddit.ToHttpContent();

                var token = await _helper.AcquireTokenSilentAsync();


                if (string.IsNullOrWhiteSpace(token))
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized).ToString();
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await h.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var location = response.Headers.GetValues("Location").FirstOrDefault();
                    return location;
                }
                return null;

            }
        }

        public async Task<bool> DeleteAsync(string subredditName)
        {
            var response = await _client.DeleteAsync($"api/subreddit/{subredditName}");

            return response.IsSuccessStatusCode;
        }

     
        public async Task<Entities.Subreddit> FindAsync(string subredditName)
        {
            var response = await _client.GetAsync($"api/subreddit/{subredditName}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.To<Entities.Subreddit>();
            }

            return null;
        }

        public async Task<IReadOnlyCollection<Entities.Subreddit>> ReadAsync()
        {
            var response = await _client.GetAsync("api/subreddit");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.To<IReadOnlyCollection<Entities.Subreddit>>();
            }

            return null;
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
