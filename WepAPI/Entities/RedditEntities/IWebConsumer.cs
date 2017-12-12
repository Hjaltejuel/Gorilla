using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Entities.RedditEntities
{
    public interface IWebConsumer
    {
        string BaseUrl { get; }
        Task<HttpResponseMessage> Get(string uri);
        Task<HttpResponseMessage> Post(string uri, HttpContent o);
        //HttpResponseMessage Delete();
        //HttpResponseMessage Put();

    }
}
