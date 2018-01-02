using System;
using GoLava.ApplePie.Contracts.Converters;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    /// <summary>
    /// Information about a program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether auto renew is enabled.
        /// </summary>
        public bool AutoRenew { get; set; }

        /// <summary>
        /// Gets or sets the date when the program is about to expire.
        /// </summary>
        [JsonConverter(typeof(MillisecondsEpochConverter))]
        public DateTime DateExpires { get; set; }

        /// <summary>
        /// Gets or sets the auto renew price.
        /// </summary>
        public string AutoRenewPrice { get; set; }
    }
}