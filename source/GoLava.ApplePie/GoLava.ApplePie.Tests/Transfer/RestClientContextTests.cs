using GoLava.ApplePie.Transfer;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer
{
    public class RestClientContextTests
    {
        [Fact]
        public void CookieJarIsNotNull()
        {
            var context = new RestClientContext();
            Assert.NotNull(context.CookieJar);
        }
    }
}