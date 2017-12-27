using System.Collections.Generic;
using System.Threading.Tasks;
using GoLava.ApplePie.Transfer.Content;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer.Content
{
    public class CustomFormUrlEncodedContentTests
    {
        [Fact]
        public async Task NullWillCreateEmptyContent()
        {
            var encodedContent = new CustomFormUrlEncodedContent(null);
            var result = await encodedContent.ReadAsStringAsync();
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task KeyValueCollectionWillBeAddedAsExpected()
        {
            var encodedContent = new CustomFormUrlEncodedContent(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("foo", "bar"),
                new KeyValuePair<string, string>("bar", "föö")
            });
            var result = await encodedContent.ReadAsStringAsync();
            Assert.Equal("foo=bar&bar=f%C3%B6%C3%B6", result);
        }

        [Fact]
        public async Task PropertiesWillBeAddedAsExpected()
        {
            var encodedContent = new CustomFormUrlEncodedContent(new {
                foo = "bar",
                Bar = "föö"
            });
            var result = await encodedContent.ReadAsStringAsync();
            Assert.Equal("foo=bar&bar=f%C3%B6%C3%B6", result);
        }
    }
}
