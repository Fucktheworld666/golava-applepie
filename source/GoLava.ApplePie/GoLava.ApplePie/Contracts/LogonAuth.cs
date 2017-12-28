using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// A container of logon authentication result properties.
    /// </summary>
    public class LogonAuth : DeviceVerification
    {
        /// <summary>
        /// Gets or sets a list of device verifications.
        /// </summary>
        /// <value>The device verifications.</value>
        public List<DeviceVerification> DeviceVerifications { get; set; }

        /// <summary>
        /// Gets or sets the verifiable device.
        /// </summary>
        /// <value>The verifiable device.</value>
        public TrustedDevice VerifiableDevice { get; set; }

        /// <summary>
        /// Gets a value indicating whether two step authentication is required.
        /// </summary>
        /// <value><c>true</c> if two step authentication is required; otherwise, <c>false</c>.</value>
        [JsonIgnore]
        public bool IsTwoStepRequired 
            => this.AuthType.Equals("hsa", StringComparison.InvariantCultureIgnoreCase);
    }
}