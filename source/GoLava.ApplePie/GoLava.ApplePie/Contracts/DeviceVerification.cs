using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts
{
    public class DeviceVerification
    {
        public List<TrustedDevice> TrustedDevices { get; set; }

        public bool SecurityCodeLocked { get; set; }

        public bool RecoveryKeyLocked { get; set; }

        public string CantSignInWith2SVHelpLink { get; set; }

        public string ManageTrustedDevicesLinkName { get; set; }

        public bool SuppressResend { get; set; }

        public bool AccountLocked { get; set; }

        public string AuthType { get; set; }

        public SecurityCode SecurityCode { get; set; }
    }
}