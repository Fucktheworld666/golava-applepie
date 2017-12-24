using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts
{
    public class Session
    {
        public User User { get; set; }

        public Provider Provider { get; set; }

        public List<Provider> AvailableProviders { get; set; }
    }
}