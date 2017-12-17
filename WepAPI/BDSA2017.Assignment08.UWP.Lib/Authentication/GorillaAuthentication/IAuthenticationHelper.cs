using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace UITEST.Authentication.GorillaAuthentication
{
    public interface IAuthenticationHelper
    {
        Task<WebAccount> SignInAsync();
        Task SignOutAsync(WebAccount account);
        Task<string> AcquireTokenSilentAsync();
        Task<WebAccount> GetAccountAsync();
    }
}
