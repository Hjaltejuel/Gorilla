using System;

namespace UITEST.Authentication.RedditAuthentication
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
