using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace UI.Lib.Authentication.GorillaAuthentication
{
    public interface IAuthenticationHelper
    {
        Task<WebAccount> SignInAsync();
        Task SignOutAsync(WebAccount account);
        Task<string> AcquireTokenSilentAsync();
        Task<WebAccount> GetAccountAsync();
    }
}
