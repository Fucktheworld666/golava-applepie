namespace GoLava.ApplePie.Clients
{
    /// <summary>
    /// A URL provider interface for all basic URLs.
    /// </summary>
    public interface IUrlProvider
    {
        /// <summary>
        /// Gets the authentication token URL.
        /// </summary>
        string AuthTokenUrl { get; }

        /// <summary>
        /// Gets the logon URL.
        /// </summary>
        string LogonUrl { get; }

        /// <summary>
        /// Gets the session URL.
        /// </summary>
        string SessionUrl { get; }

        /// <summary>
        /// Gets the two step authentication URL.
        /// </summary>
        string TwoStepAuthUrl { get; }

        /// <summary>
        /// Gets the two step verification URL.
        /// </summary>
        string TwoStepVerifyUrl { get; }
    }
}