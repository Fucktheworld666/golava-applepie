namespace GoLava.ApplePie.Clients
{
    public abstract class UrlProviderBase : IUrlProvider
    {
        public string AuthTokenUrl => "https://olympus.itunes.apple.com/v1/app/config?hostname=itunesconnect.apple.com";

        public string LogonUrl => "https://idmsa.apple.com/appleauth/auth/signin";

        public string SessionUrl => "https://olympus.itunes.apple.com/v1/session";
    }
}
