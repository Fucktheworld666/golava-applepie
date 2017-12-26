using GoLava.ApplePie.Transfer;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer
{
    public class RestUriTests
    {
        [Fact]
        public void NamedParametersAreReplaced()
        {
            var restUri = new RestUri("http://domain.ext/{foo}/index.{ext}", new { foo = "bar", ext = "html" });
            Assert.Equal("http://domain.ext/bar/index.html", restUri.AbsoluteUri);
        }
    }
}
