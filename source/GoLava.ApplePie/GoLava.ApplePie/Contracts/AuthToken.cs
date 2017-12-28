namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// An authentication token.
    /// </summary>
    public class AuthToken
    {
        /// <summary>
        /// Gets or sets the authentication service key.
        /// </summary>
        public string AuthServiceKey { get; set; }

        /// <summary>
        /// Gets or sets the authentication service URL.
        /// </summary>
        public string AuthServiceUrl { get; set; }
    }
}