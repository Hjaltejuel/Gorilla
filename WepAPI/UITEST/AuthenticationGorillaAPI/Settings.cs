using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace Gorilla.AuthenticationGorillaAPI
{
    public class Settings : ISettings
    {
        public string Tenant => "ituniversity.onmicrosoft.com";

        public string ClientId => "a562a247-2010-4e1e-82dd-8133a37b346a";

        public string RedirectUri => $"ms-appx-web://Microsoft.AAD.BrokerPlugIn/{WebAuthenticationBroker.GetCurrentApplicationCallbackUri().Host.ToUpper()}";

        public string Instance => "https://login.microsoftonline.com/";

        public string WebAccountProviderId => "https://login.microsoft.com";

        public string ApiResource => "https://ituniversity.onmicrosoft.com/GorillaAPI";

        public Uri ApiBaseAddress => new Uri("http://gorillaapi.azurewebsites.net");

        public string Authority => $"{Instance}{Tenant}";
    }
}
