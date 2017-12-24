using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts.Portal
{
    public class NewDevices
    {
        public List<string> DeviceNumbers { get; set; }

        public List<string> DeviceNames { get; set; }

        public string Register { get; set; }

        public List<DeviceClass> DeviceClasses { get; set; }
    }
}