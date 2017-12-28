using System;
using GoLava.ApplePie.Contracts.Converters;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// Information about a trusted device.
    /// </summary>
    public class TrustedDevice
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:TrustedDevice"/> is trusted.
        /// </summary>
        /// <value><c>true</c> if trusted; otherwise, <c>false</c>.</value>
        public bool Trusted { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the live status.
        /// </summary>
        /// <value>The live status.</value>
        public string LiveStatus { get; set; }

        /// <summary>
        /// Gets or sets the last two digits.
        /// </summary>
        public string LastTwoDigits { get; set; }

        /// <summary>
        /// Gets or sets a image location that has a scale factor of 1.
        /// </summary>
        public string ImageLocation { get; set; }

        /// <summary>
        /// Gets or sets a image location that has a scale factor of 2.
        /// </summary>
        public string ImageLocation2x { get; set; }

        /// <summary>
        /// Gets or sets a image location that has a scale factor of 3.
        /// </summary>
        public string ImageLocation3x { get; set; }

        /// <summary>
        /// Gets or sets the date when this <see cref="T:TrustedDevice"/> 
        /// information was last updated..
        /// </summary>
        [JsonConverter(typeof(MillisecondsEpochConverter))]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the number with area code country dialing code.
        /// </summary>
        public string NumberWithAreaCodeCountryDialingCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:TrustedDevice"/> is the same device
        /// that used to make the request.
        /// </summary>
        public bool ThisDevice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:GoLava.ApplePie.Contracts.TrustedDevice"/> is online.
        /// </summary>
        public bool Online { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:GoLava.ApplePie.Contracts.TrustedDevice"/> is a virtual device.
        /// </summary>
        public bool VirtualDevice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:GoLava.ApplePie.Contracts.TrustedDevice"/> is a device.
        /// </summary>
        public bool Device { get; set; }
    }
}