using System.Security;
using GoLava.ApplePie.Contracts.Converters;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts
{
    public abstract class Credentials
    {
        public string AccountName { get; set; }

        public bool RememberMe { get; } = true;
    }

    public class NonSecureCredentials : Credentials
    {
        public string Password { get; set; }
    }

    public class SecureCredentials : Credentials
    {
        [JsonConverter(typeof(SecureStringConverter))]
        public SecureString Password { get; set; }
    }
}
