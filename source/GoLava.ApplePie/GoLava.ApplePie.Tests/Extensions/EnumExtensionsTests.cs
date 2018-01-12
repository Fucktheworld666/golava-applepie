using System.ComponentModel;
using System.Runtime.Serialization;
using GoLava.ApplePie.Extensions;
using Xunit;

namespace GoLava.ApplePie.Tests.Extensions
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void ToStringValueReturnsToStringWithoutEnumMemberAttribute()
        {
            var a = TestEnum.A;
            Assert.Equal(a.ToString(), a.ToStringValue());
        }

        [Fact]
        public void ToStringValueReturnsEnumMemberAttribute()
        {
            var b = TestEnum.B;
            Assert.Equal("x", b.ToStringValue());
        }

        private enum TestEnum
        {
            A,
            [EnumMember(Value = "x")]B
        }
    }
}
