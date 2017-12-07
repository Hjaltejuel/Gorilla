﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApplication2.Models.GorillaApiConsumeRepositories
{ 
    public class RestUserPreferenceRepository : IUserPreferenceRepository
    {
        private readonly Uri _baseAddress = new Uri("http://gorillaapi.azurewebsites.net/");

        private readonly HttpClient _client;

        public RestUserPreferenceRepository(HttpClient client)
        {
            client.BaseAddress = _baseAddress;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _client = client;
        }
        public async Task<(string, string)> CreateAsync(UserPreference userPreference)
        {
            var response = await _client.PostAsync("api/UserPreference/", userPreference.ToHttpContent());
            if (response.IsSuccessStatusCode)
            {
                var location = response.Headers.GetValues("Location").First();
                return (location,null);
            }

            return (null,null);
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
                return await response.Content.To<IReadOnlyCollection<Entities.UserPreference>>();
            }

            return null;
        }

        public async Task<bool> UpdateAsync(UserPreference userPreference)
        {
            var response = await _client.PutAsync($"api/UserPreference/", userPreference.ToHttpContent());

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