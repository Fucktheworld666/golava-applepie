using GoLava.ApplePie.Security;
using Xunit;

namespace GoLava.ApplePie.Tests.Security
{
    public class SecureRandomProviderTests
    {
        [Fact]
        public void GetSecureRandomReturnsSameInstance()
        {
            var secureRandomProvider = new SecureRandomProvider();
            var sr1 = secureRandomProvider.GetSecureRandom();
            var sr2 = secureRandomProvider.GetSecureRandom();

            Assert.Same(sr1, sr2);
        }

        [Fact]
        public void GetSecureRandomReturnsAlwaysSameInstance()
        {
            var secureRandomProvider1 = new SecureRandomProvider();
            var sr1 = secureRandomProvider1.GetSecureRandom();

            var secureRandomProvider2 = new SecureRandomProvider();
            var sr2 = secureRandomProvider2.GetSecureRandom();

            Assert.Same(sr1, sr2);
        }
    }
}
