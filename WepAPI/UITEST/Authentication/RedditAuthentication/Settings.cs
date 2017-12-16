using System;
using Windows.Security.Authentication.Web;

namespace UITEST.Authentication.RedditAuthentication
{
    public class Settings : ISettings
    {
        public string Tenant => "http://www.reddit.com/";

        public string ClientId => "ephxxGR7ZA77nA";

        public string RedirectUri => $"ms-appx-web://Microsoft.AAD.BrokerPlugIn/{WebAuthenticationBroker.GetCurrentApplicationCallbackUri().Host.ToUpper()}";

        public string Instance => "https://login.microsoftonline.com/";

        public string WebAccountProviderId => "https://login.microsoft.com";

        public string ApiResource => "https://ituniversity.onmicrosoft.com/GorillaAPI";

        public Uri ApiBaseAddress => new Uri("http://gorillaapi.azurewebsites.net");

        public string Authority => $"{Instance}{Tenant}";
    }
}
