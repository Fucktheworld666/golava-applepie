using System.ComponentModel;
using GoLava.ApplePie.Extensions;
using Xunit;

namespace GoLava.ApplePie.Tests.Extensions
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void ToDescriptionStringReturnsToStringWithoutDescriptionAttribute()
        {
            var a = TestEnum.A;
            Assert.Equal(a.ToString(), a.ToDescriptionString());
        }

        [Fact]
        public void ToDescriptionStringReturnsDescriptionAttribute()
        {
            var b = TestEnum.B;
            Assert.Equal("x", b.ToDescriptionString());
        }

        private enum TestEnum
        {
            A,
            [Description("x")]B
        }
    }
}
