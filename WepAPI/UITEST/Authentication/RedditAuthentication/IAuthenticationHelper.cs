using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace UITEST.Authentication.RedditAuthentication
{
    public interface IAuthenticationHelper
    {
        Task<WebAccount> SignInAsync();
        Task SignOutAsync(WebAccount account);
        Task<string> AcquireTokenSilentAsync();
        Task<WebAccount> GetAccountAsync();
    }
}
