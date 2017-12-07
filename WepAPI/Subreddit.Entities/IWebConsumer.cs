using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Subreddit.Entities
{
    interface IWebConsumer
    {
        string BaseUrl { get; set; }
        Task<HttpResponseMessage> Get(Uri uri);
        Task<HttpResponseMessage> Post(Uri uri, Object o);
        //HttpResponseMessage Delete();
        //HttpResponseMessage Put();

    }
}
