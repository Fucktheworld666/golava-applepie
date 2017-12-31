using System.ComponentModel;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    public enum DataProtectionPermissionLevel
    {
        [Description("complete")]
        Complete,
        [Description("unlessopen")]
        UnlessOpen,
        [Description("untilfirstauth")]
        UntilFirstAuth
    }
}