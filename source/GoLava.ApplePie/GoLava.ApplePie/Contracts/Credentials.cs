using GoLava.ApplePie.Contracts.Attributes;

namespace GoLava.ApplePie.Contracts
{
    public class Credentials
    {
        public string AccountName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get => true; }
    }
}