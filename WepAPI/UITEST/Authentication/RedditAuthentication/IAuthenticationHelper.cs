using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace Gorilla.RedditAuthentication
{
    public interface IAuthenticationHelper
    {
        Task<WebAccount> SignInAsync();
        Task SignOutAsync(WebAccount account);
        Task<string> AcquireTokenSilentAsync();
        Task<WebAccount> GetAccountAsync();
    }
}
