namespace GoLava.ApplePie.Clients
{
    /// <summary>
    /// Base implementation of the <see cref="IUrlProvider"/> interface.
    /// </summary>
    public abstract class UrlProviderBase : IUrlProvider
    {
        /// <summary>
        /// Gets the authentication token URL.
        /// </summary>
        public string AuthTokenUrl => "https://olympus.itunes.apple.com/v1/app/config?hostname=itunesconnect.apple.com";

        /// <summary>
        /// Gets the logon URL.
        /// </summary>
        public string LogonUrl => "https://idmsa.apple.com/appleauth/auth/signin";

        /// <summary>
        /// Gets the session URL.
        /// </summary>
        public string SessionUrl => "https://olympus.itunes.apple.com/v1/session";

        /// <summary>
        /// Gets the two step authentication URL.
        /// </summary>
        public string TwoStepAuthUrl => "https://idmsa.apple.com/appleauth/auth";

        /// <summary>
        /// Gets the two step verification URL.
        /// </summary>
        public string TwoStepVerifyUrl => "https://idmsa.apple.com/appleauth/auth/verify/device/{deviceId}/securitycode";
    }
}