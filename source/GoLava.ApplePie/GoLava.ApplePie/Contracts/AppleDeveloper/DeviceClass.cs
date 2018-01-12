using System.Runtime.Serialization;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    public enum DeviceClass
    {
        [EnumMember(Value = "watch")]
        AppleWatch,
        [EnumMember(Value = "ipad")]
        iPad,
        [EnumMember(Value = "iphone")]
        iPhone,
        [EnumMember(Value = "ipod")]
        iPod,
        [EnumMember(Value = "tvOS")]
        AppleTV
    }
}