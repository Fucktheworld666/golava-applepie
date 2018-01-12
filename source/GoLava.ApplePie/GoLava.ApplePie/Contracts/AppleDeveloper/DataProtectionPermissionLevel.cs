using System.Runtime.Serialization;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    public enum DataProtectionPermissionLevel
    {
        [EnumMember(Value = "complete")]
        Complete,
        [EnumMember(Value = "unlessopen")]
        UnlessOpen,
        [EnumMember(Value = "untilfirstauth")]
        UntilFirstAuth
    }
}