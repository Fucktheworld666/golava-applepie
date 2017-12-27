using System.Net;
using GoLava.ApplePie.Transfer;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer
{
    public class RestResponseTests
    {
        [Fact]
        public void HeadersIsNullByDefault()
        {
            var restResponse = new RestResponse<string>();
            Assert.Null(restResponse.Headers);
        }

        [Fact]
        public void RawContentIsNullByDefault()
        {
            var restResponse = new RestResponse<string>();
            Assert.Null(restResponse.RawContent);
        }

        [Fact]
        public void ContentIsNullByDefault()
        {
            var restResponse = new RestResponse<string>();
            Assert.Null(restResponse.Content);
        }

        [Fact]
        public void IsSuccessIsFalseByDefault()
        {
            var restResponse = new RestResponse<string>();
            Assert.False(restResponse.IsSuccess);
        }

        [Fact]
        public void StatusCodeIsZeroByDefault()
        {
            var restResponse = new RestResponse<string>();
            Assert.Equal(0, (int)restResponse.StatusCode);
        }

        [Fact]
        public void ContentTypeIsNoneByDefault()
        {
            var restResponse = new RestResponse<string>();
            Assert.Equal(RestContentType.None, restResponse.ContentType);
        }

        [Theory]
        [InlineData(200, 299)]
        public void IsSucessIsTrueFor(int min, int max)
        {
            for (var i = min; i <= max; i++)
            {
                var restResponse = new RestResponse<string>
                {
                    StatusCode = (HttpStatusCode)i
                };
                Assert.True(restResponse.IsSuccess, "IsSuccess is false for " + (HttpStatusCode)i);
            }
        }


        [Theory]
        [InlineData(0, 199)]
        [InlineData(300, 1000)]
        public void IsSucessIsFalseFor(int min, int max)
        {
            for (var i = min; i <= max; i++)
            {
                var restResponse = new RestResponse<string>
                {
                    StatusCode = (HttpStatusCode)i
                };
                Assert.False(restResponse.IsSuccess, "IsSuccess is true for " + (HttpStatusCode)i);
            }
        }
    }
}