using Model;
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
        public Task<(string, string)> CreateAsync(UserPreference userPreference)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string username, string subredditName)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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

        public Task<bool> UpdateAsync(UserPreference userPreference)
        {
            throw new NotImplementedException();
        }
    }
}
