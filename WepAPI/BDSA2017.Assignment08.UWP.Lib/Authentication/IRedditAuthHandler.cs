using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UI.Lib.Authentication
{
    public interface IRedditAuthHandler
    {
        Task BeginAuth();
        HttpRequestMessage AuthenticateRequest(HttpRequestMessage request);
        Task<JToken> SendRequest(string body);
        void LogOut();
        Task RefreshToken();
        Task Authenticate(string data);
        string GetAuthCode(string data);
    }
}
