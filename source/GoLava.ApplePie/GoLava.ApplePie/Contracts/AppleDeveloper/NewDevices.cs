using System.Collections.Generic;
using GoLava.ApplePie.Contracts.Attributes;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    [CsrfClass(CsrfClass.Device)]
    public class NewDevices
    {
        public List<string> DeviceNumbers { get; set; }

        public List<string> DeviceNames { get; set; }

        public string Register { get; set; }

        public List<DeviceClass> DeviceClasses { get; set; }
    }
}