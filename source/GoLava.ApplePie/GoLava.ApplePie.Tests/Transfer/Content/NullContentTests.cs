using System.Threading.Tasks;
using GoLava.ApplePie.Transfer.Content;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer.Content
{
    public class NullContentTests
    {
        [Fact]
        public void ContentLengthIsZero()
        {
            var nullContent = new NullContent();
            Assert.Equal(0, nullContent.Headers.ContentLength);
        }

        [Fact]
        public void ContentTypeIsNull()
        {
            var nullContent = new NullContent();
            Assert.Null(nullContent.Headers.ContentType);
        }

        [Fact]
        public void ContentDispositionIsNull()
        {
            var nullContent = new NullContent();
            Assert.Null(nullContent.Headers.ContentDisposition);
        }

        [Fact]
        public void AllowIsEmpty()
        {
            var nullContent = new NullContent();
            Assert.Equal(0, nullContent.Headers.Allow.Count);
        }

        [Fact]
        public void ContentEncodingIsEmpty()
        {
            var nullContent = new NullContent();
            Assert.Equal(0, nullContent.Headers.ContentEncoding.Count);
        }

        [Fact]
        public void ContentLanguageIsEmpty()
        {
            var nullContent = new NullContent();
            Assert.Equal(0, nullContent.Headers.ContentLanguage.Count);
        }

        [Fact]
        public void ContentMD5IsNull()
        {
            var nullContent = new NullContent();
            Assert.Null(nullContent.Headers.ContentMD5);
        }

        [Fact]
        public void ContentRangeIsNull()
        {
            var nullContent = new NullContent();
            Assert.Null(nullContent.Headers.ContentRange);
        }

        [Fact]
        public async Task ReadAsStringAsyncReturnsEmptyString()
        {
            var nullContent = new NullContent();
            var result = await nullContent.ReadAsStringAsync();
            Assert.Equal(string.Empty, result);
        }
    }
}