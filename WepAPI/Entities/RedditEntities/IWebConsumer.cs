using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Entities.RedditEntities
{
    public interface IWebConsumer
    {
        string BaseUrl { get; }
        Task<HttpResponseMessage> Get(string uri, string auth);
        Task<HttpResponseMessage> Post(string uri, HttpContent o);
        //HttpResponseMessage Delete();
        //HttpResponseMessage Put();

    }
}
