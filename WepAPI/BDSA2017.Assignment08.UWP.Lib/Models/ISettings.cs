using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorilla.AuthenticationGorillaAPI
{
    public interface ISettings
    {
        Uri ApiBaseAddress { get; }
        string ApiResource { get; }
        string Authority { get; }
        string ClientId { get; }
        string Instance { get; }
        string RedirectUri { get; }
        string Tenant { get; }
        string WebAccountProviderId { get; }
    }
}
