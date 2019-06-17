using GoLava.ApplePie.Extensions;
using Xunit;

namespace GoLava.ApplePie.Tests.Extensions
{
    public class ByteListExtensionsTests
    {
        [Fact]
        public void ToHexString()
        {
            var bytes = new byte[] { 0x01, 0x02, 0x03, 0x0D, 0x0E, 0x0F };
            var actual = bytes.ToHexString();
            Assert.Equal("0102030d0e0f", actual);
        }

        [Fact]
        public void ToHexCharArray()
        {
            var bytes = new byte[] { 0x01, 0x02, 0x03, 0x0D, 0x0E, 0x0F };
            var actual = bytes.ToHexCharArray();
            Assert.Equal(new char[] { '0', '1', '0', '2', '0', '3', '0', 'd', '0', 'e', '0', 'f' }, actual);
        }
    }
}
