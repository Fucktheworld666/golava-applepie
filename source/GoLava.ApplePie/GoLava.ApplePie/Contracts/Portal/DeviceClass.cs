using System.ComponentModel;

namespace GoLava.ApplePie.Contracts.Portal
{
    public enum DeviceClass
    {
        [Description("watch")]
        AppleWatch,
        [Description("ipad")]
        iPad,
        [Description("iphone")]
        iPhone,
        [Description("ipod")]
        iPod,
        [Description("tvOS")]
        AppleTV
    }
}