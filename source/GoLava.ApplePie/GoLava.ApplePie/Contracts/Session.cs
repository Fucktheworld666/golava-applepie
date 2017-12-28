using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// Session data that is retrieved after logon.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Gets or sets the user information of the session.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the current provider of the session.
        /// </summary>
        public Provider Provider { get; set; }

        /// <summary>
        /// Gets or sets the available providers.
        /// </summary>
        public List<Provider> AvailableProviders { get; set; }
    }
}