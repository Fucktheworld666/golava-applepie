using System.Collections.Generic;
using GoLava.ApplePie.Contracts.Attributes;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    [CsrfClass(CsrfClass.Device)]
    public class DevicesResult : PageResult
    {
        public List<Device> Devices { get; set; }

        public override int RecordsCount
        {
            get => this.Devices != null ? this.Devices.Count : 0;
        }
    }
}