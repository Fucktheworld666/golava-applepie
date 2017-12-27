using System;
using System.Net.Http;
using System.Text;
using GoLava.ApplePie.Transfer;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer
{
    public class RestRequestTests
    {
        [Fact]
        public void MethodIsNullByDefault()
        {
            var restRequest = new RestRequest();
            Assert.Null(restRequest.Method);
        }

        [Fact]
        public void ContentIsNullByDefault()
        {
            var restRequest = new RestRequest();
            Assert.Null(restRequest.Content);
        }

        [Fact]
        public void ContentEncodingIsNullByDefault()
        {
            var restRequest = new RestRequest();
            Assert.Null(restRequest.ContentEncoding);
        }

        [Fact]
        public void HeadersIsNullByDefault()
        {
            var restRequest = new RestRequest();
            Assert.Null(restRequest.Headers);
        }

        [Fact]
        public void UriIsNullByDefault()
        {
            var restRequest = new RestRequest();
            Assert.Null(restRequest.Uri);
        }

        [Fact]
        public void ContentTypeIsNoneByDefault()
        {
            var restRequest = new RestRequest();
            Assert.Equal(RestContentType.None, restRequest.ContentType);
        }

        [Fact]
        public void GetSetsMethodToGet()
        {
            var restRequest = RestRequest.Get(new RestUri("http://domain.ext"));
            Assert.Equal(HttpMethod.Get, restRequest.Method);
        }

        [Fact]
        public void GetSetsUri()
        {
            var restRequest = RestRequest.Get(new RestUri("http://domain.ext"));
            Assert.Equal("http://domain.ext/", restRequest.Uri.AbsoluteUri);
        }

        [Fact]
        public void GetSetsHeaders()
        {
            var headers = new RestHeaders();
            var restRequest = RestRequest.Get(new RestUri("http://domain.ext"), headers);
            Assert.Same(headers, restRequest.Headers);
        }

        [Fact]
        public void PostSetsMethodToPost()
        {
            var restRequest = RestRequest.Post(new RestUri("http://domain.ext"));
            Assert.Equal(HttpMethod.Post, restRequest.Method);
        }

        [Fact]
        public void PostSetsUri()
        {
            var restRequest = RestRequest.Post(new RestUri("http://domain.ext"));
            Assert.Equal("http://domain.ext/", restRequest.Uri.AbsoluteUri);
        }

        [Fact]
        public void PostSetsContentEncodingToUtf8()
        {
            var restRequest = RestRequest.Post(new RestUri("http://domain.ext"));
            Assert.Equal(Encoding.UTF8, restRequest.ContentEncoding);
        }

        [Fact]
        public void PostSetsHeaders()
        {
            var headers = new RestHeaders();
            var restRequest = RestRequest.Post(new RestUri("http://domain.ext"), headers);
            Assert.Same(headers, restRequest.Headers);
        }

        [Fact]
        public void PostSetsContent()
        {
            var content = new object();
            var restRequest = RestRequest.Post(new RestUri("http://domain.ext"), RestContentType.Json, content);
            Assert.Same(content, restRequest.Content);
        }

        [Fact]
        public void PostSetsContentType()
        {
            var content = new object();
            var restRequest = RestRequest.Post(new RestUri("http://domain.ext"), RestContentType.Json, content);
            Assert.Equal(RestContentType.Json, restRequest.ContentType);
        }

        [Fact]
        public void PutSetsMethodToPut()
        {
            var restRequest = RestRequest.Put(new RestUri("http://domain.ext"));
            Assert.Equal(HttpMethod.Put, restRequest.Method);
        }

        [Fact]
        public void PutSetsUri()
        {
            var restRequest = RestRequest.Put(new RestUri("http://domain.ext"));
            Assert.Equal("http://domain.ext/", restRequest.Uri.AbsoluteUri);
        }

        [Fact]
        public void PutSetsContentEncodingToUtf8()
        {
            var restRequest = RestRequest.Put(new RestUri("http://domain.ext"));
            Assert.Equal(Encoding.UTF8, restRequest.ContentEncoding);
        }

        [Fact]
        public void PutSetsHeaders()
        {
            var headers = new RestHeaders();
            var restRequest = RestRequest.Put(new RestUri("http://domain.ext"), headers);
            Assert.Same(headers, restRequest.Headers);
        }

        [Fact]
        public void PutSetsContent()
        {
            var content = new object();
            var restRequest = RestRequest.Put(new RestUri("http://domain.ext"), RestContentType.Json, content);
            Assert.Same(content, restRequest.Content);
        }

        [Fact]
        public void PutSetsContentType()
        {
            var content = new object();
            var restRequest = RestRequest.Put(new RestUri("http://domain.ext"), RestContentType.Json, content);
            Assert.Equal(RestContentType.Json, restRequest.ContentType);
        }
    }
}
