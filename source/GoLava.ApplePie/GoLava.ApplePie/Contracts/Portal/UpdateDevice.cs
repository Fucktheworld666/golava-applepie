using GoLava.ApplePie.Contracts.Attributes;

namespace GoLava.ApplePie.Contracts.Portal
{
    [CsrfClass(CsrfClass.Device)]
    public class UpdateDevice
    {
        public string DeviceId { get; set; }

        public string DeviceNumber { get; set; }

        public string Name { get; set; }
    }
}
