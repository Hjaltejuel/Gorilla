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
    public class RestUserRepository : IUserRepository
    {
        private readonly Uri _baseAddress = new Uri("http://gorillaapi.azurewebsites.net/");

        private readonly HttpClient _client;

        public RestUserRepository(HttpClient client)
        {
            client.BaseAddress = _baseAddress;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _client = client;
        }
        public Task<string> CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string username)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<User> FindAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<User>> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
