using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    class RestPostRepository : IRestPostRepository
    {
        private readonly HttpClient _client;
        private readonly IAuthenticationHelper _helper;

        public RestPostRepository(ISettings settings, DelegatingHandler handler, IAuthenticationHelper helper)
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

        public async Task<string> CreateAsync(Post post)
        {
            using (var h = new HttpClient())
            {
                var request = new HttpRequestMessage(new HttpMethod("POST"),
                    new Uri("https://gorillaapi.azurewebsites.net/api/post")) {Content = post.ToHttpContent()};

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

       

        public async Task<IReadOnlyCollection<Post>> ReadAsync(string username)
        {
            var response = await _client.GetAsync($"api/post/{username}");

            if (response.IsSuccessStatusCode)
            {
                var reponse = await response.Content.To<IReadOnlyCollection<Post>>();
                if (reponse == null)
                {
                    var list = new List<Post>();
                    var emptyList = new ReadOnlyCollection<Post>(list);
                    return emptyList;
                }
                return reponse;
            }
            return null;
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
