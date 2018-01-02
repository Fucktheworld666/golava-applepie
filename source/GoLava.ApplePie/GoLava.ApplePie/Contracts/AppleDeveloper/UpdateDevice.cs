using GoLava.ApplePie.Contracts.Attributes;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    /// <summary>
    /// A contract to update a device.
    /// </summary>
    [CsrfClass(CsrfClass.Device)]
    public class UpdateDevice
    {
        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the device number.
        /// </summary>
        public string DeviceNumber { get; set; }

        /// <summary>
        /// Gets or sets the new name of the devices.
        /// </summary>
        public string Name { get; set; }
    }
}
