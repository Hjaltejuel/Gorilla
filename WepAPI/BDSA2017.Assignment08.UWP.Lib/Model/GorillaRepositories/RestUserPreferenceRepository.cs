﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Entities.GorillaEntities;
using UI.Lib.Authentication.GorillaAuthentication;
using UI.Lib.Model.GorillaRestInterfaces;

namespace UI.Lib.Model.GorillaRepositories
{
    public class RestUserPreferenceRepository : IRestUserPreferenceRepository
    {
        private readonly HttpClient _client;

        private readonly IAuthenticationHelper _helper;

        public RestUserPreferenceRepository(ISettings settings, DelegatingHandler handler, IAuthenticationHelper helper)
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

        public async Task<string> CreateAsync(UserPreference userPreference)
        {
            using (var h = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"),
                    new Uri("https://gorillaapi.azurewebsites.net/api/Userpreference"))
                {
                    Content = userPreference.ToHttpContent()
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

        public async Task<bool> DeleteAsync(string username, string subredditName)
        {
            var response = await _client.DeleteAsync($"api/UserPreference/{username}{subredditName}");

            return response.IsSuccessStatusCode;
        }

        public async Task<IReadOnlyCollection<UserPreference>> FindAsync(string username)
        {
            var response = await _client.GetAsync($"api/UserPreference/{username}");

            if (response.IsSuccessStatusCode)
            {
                
                return await response.Content.To<IReadOnlyCollection<UserPreference>>();
            }

            return null;
        }

        public async Task<bool> UpdateAsync(UserPreference userPreference)
        {
            var response = await _client.PutAsync("api/UserPreference/", userPreference.ToHttpContent());

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
