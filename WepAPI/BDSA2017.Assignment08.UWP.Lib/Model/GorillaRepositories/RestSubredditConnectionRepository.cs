using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Entities.GorillaEntities;
using UI.Lib.Model.GorillaRestInterfaces;
using UI.Lib.Authentication.GorillaAuthentication;

namespace UI.Lib.Model.GorillaRepositories
{
    public class RestSubredditConnectionRepository: IRestSubredditConnectionRepository
    {
        private readonly IAuthenticationHelper _helper;

        private readonly HttpClient _client;

        public RestSubredditConnectionRepository(ISettings settings, DelegatingHandler handler, IAuthenticationHelper helper)
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

        public async Task<string> CreateAsync(SubredditConnection subredditConnection)
        {
            using (var h = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"),
                    new Uri("https://gorillaapi.azurewebsites.net/api/SubredditConnection"))
                {
                    Content = subredditConnection.ToHttpContent()
                };

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

        public async Task<IReadOnlyCollection<SubredditConnection>> GetAllPrefs(string[] subredditFromNames)
        {
            using (var h = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"),
                    new Uri("https://gorillaapi.azurewebsites.net/api/SubredditConnection/GetAllPrefs"))
                {
                    Content = subredditFromNames.ToHttpContent()
                };

                var token = await _helper.AcquireTokenSilentAsync();


                if (string.IsNullOrWhiteSpace(token))
                {
                    return null;
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await h.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                   return await response.Content.To<IReadOnlyCollection<SubredditConnection>>();
                }
                return null;

            }
        }

        public async Task<IReadOnlyCollection<SubredditConnection>> FindAsync(string subredditFromName)
        {
            Debug.WriteLine("I AM Begining");
            var response = await _client.GetAsync($"api/subredditConnection/{subredditFromName}");
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("I AM STARTING");
                return await response.Content.To<IReadOnlyCollection<SubredditConnection>>();
            }
            return null;
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
            var response = await _client.PutAsync("api/subredditConnection", subredditConnection.ToHttpContent());

            return response.IsSuccessStatusCode;
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
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
