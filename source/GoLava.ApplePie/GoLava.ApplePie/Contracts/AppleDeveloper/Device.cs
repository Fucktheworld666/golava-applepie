using GoLava.ApplePie.Contracts.Attributes;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    /// <summary>
    /// The device contract.
    /// </summary>
    [CsrfClass(CsrfClass.Device), JsonDataProperty]
    public class Device
    {
        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the device number.
        /// </summary>
        public string DeviceNumber { get; set; }

        /// <summary>
        /// Gets or sets the device platform.
        /// </summary>
        [JsonProperty("devicePlatform")]
        public Platform Platform { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the device class.
        /// </summary>
        public string DeviceClass { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the team identifier.
        /// </summary>
        [JsonIgnore]
        public string TeamId { get; internal set; }
    }
}