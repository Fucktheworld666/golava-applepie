using System.Runtime.Serialization;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    public enum Platform
    {
        [EnumMember(Value = "ios")]
        Ios,
        [EnumMember(Value = "mac")]
        Mac,
        [EnumMember(Value = "safari")]
        Safari
    }
}