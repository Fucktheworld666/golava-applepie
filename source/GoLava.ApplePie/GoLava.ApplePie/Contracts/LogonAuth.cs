using System;
using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts
{
    public class LogonAuth : DeviceVerification
    {
        public List<DeviceVerification> DeviceVerifications { get; set; }

        public TrustedDevice VerifiableDevice { get; set; }

        public bool IsTwoStepRequired 
            => this.AuthType.Equals("hsa", StringComparison.InvariantCultureIgnoreCase);
    }
}