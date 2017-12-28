using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// Device verification.
    /// </summary>
    public class DeviceVerification
    {
        /// <summary>
        /// Gets or sets a list of trusted devices.
        /// </summary>
        /// <value>The trusted devices.</value>
        public List<TrustedDevice> TrustedDevices { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the security code is locked.
        /// </summary>
        public bool SecurityCodeLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the recovery key is locked.
        /// </summary>
        public bool RecoveryKeyLocked { get; set; }

        /// <summary>
        /// Gets or sets some help link.
        /// </summary>
        public string CantSignInWith2SVHelpLink { get; set; }

        /// <summary>
        /// Gets or sets some trusted devices link.
        /// </summary>
        /// <value>The name of the manage trusted devices link.</value>
        public string ManageTrustedDevicesLinkName { get; set; }

        /// <summary>
        /// Gets or sets a value whether this resending the code is suppressed.
        /// </summary>
        /// <value><c>true</c> if suppress resend; otherwise, <c>false</c>.</value>
        public bool SuppressResend { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the account is locked.
        /// </summary>
        public bool AccountLocked { get; set; }

        /// <summary>
        /// Gets or sets the authentication type.
        /// </summary>
        public string AuthType { get; set; }

        /// <summary>
        /// Gets or sets the security code.
        /// </summary>
        public SecurityCode SecurityCode { get; set; }
    }
}