using System.Runtime.Serialization;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    public enum ApplicationType
    {
        [EnumMember(Value = "explicit")]
        Explicit,
        [EnumMember(Value = "wildcard")]
        Wildcard
    }
}
