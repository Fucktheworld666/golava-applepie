using Org.BouncyCastle.Security;

namespace GoLava.ApplePie.Security
{
    /// <summary>
    /// A secure random provider.
    /// </summary>
    public class SecureRandomProvider
    {
        private static readonly SecureRandom _secureRandom = new SecureRandom();

        /// <summary>
        /// Gets a secure random object that can be used globally.
        /// </summary>
        public SecureRandom GetSecureRandom()
        {
            return _secureRandom;
        }
    }
}