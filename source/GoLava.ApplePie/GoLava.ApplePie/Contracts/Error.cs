using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts
{
    public class Error
    {
        public List<ServiceError> ServiceErrors { get; set; }

        public string UserString { get; set; }
    }
}