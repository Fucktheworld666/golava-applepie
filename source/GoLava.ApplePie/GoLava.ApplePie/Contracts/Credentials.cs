using System.Security;
using GoLava.ApplePie.Contracts.Converters;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// Contract to hold credentials to logon to apple services.
    /// </summary>
    public abstract class Credentials
    {
        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:GoLava.ApplePie.Contracts.Credentials"/> to remember the credentials.
        /// </summary>
        public bool RememberMe { get; } = true;
    }

    /// <summary>
    /// Non secure credentials, password is stored as a plain string.
    /// </summary>
    public class NonSecureCredentials : Credentials
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// Secure credentials, password is stored as a secure string.
    /// </summary>
    public class SecureCredentials : Credentials
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [JsonConverter(typeof(SecureStringJsonConverter))]
        public SecureString Password { get; set; }
    }
}
