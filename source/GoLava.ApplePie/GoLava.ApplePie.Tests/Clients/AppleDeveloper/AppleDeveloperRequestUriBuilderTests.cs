using System;
using System.Collections.Generic;
using System.Web;
using GoLava.ApplePie.Clients.AppleDeveloper;
using Xunit;

namespace GoLava.ApplePie.Tests.Clients.AppleDeveloper
{
    public class AppleDeveloperRequestUriBuilderTests
    {
        [Fact]
        public void BuildUriHasCorrectContentTypeParameter()
        {
            var builder = new AppleDeveloperRequestUriBuilder(new Uri("http://www.foo.ext"));
            var uri = builder.ToUri();

            var query = HttpUtility.ParseQueryString(uri.Query);
            Assert.Equal("text/x-url-arguments", query["content-type"]);
        }

        [Fact]
        public void BuildUriHasCorrectAcceptParameter()
        {
            var builder = new AppleDeveloperRequestUriBuilder(new Uri("http://www.foo.ext"));
            var uri = builder.ToUri();

            var query = HttpUtility.ParseQueryString(uri.Query);
            Assert.Equal("application/json", query["accept"]);
        }

        [Fact]
        public void BuildUriHasCorrectUserLocaleParameter()
        {
            var builder = new AppleDeveloperRequestUriBuilder(new Uri("http://www.foo.ext"));
            var uri = builder.ToUri();

            var query = HttpUtility.ParseQueryString(uri.Query);
            Assert.Equal("en_US", query["userLocale"]);
        }

        [Fact]
        public void BuildUriHasRequestIdParameter()
        {
            var builder = new AppleDeveloperRequestUriBuilder(new Uri("http://www.foo.ext"));
            var uri = builder.ToUri();

            var query = HttpUtility.ParseQueryString(uri.Query);
            Assert.NotNull(query["requestId"]);
            Assert.NotEqual(string.Empty, query["requestId"]);
        }

        [Fact]
        public void RequestIdChangesWithEveryCallOfToUri()
        {
            var builder = new AppleDeveloperRequestUriBuilder(new Uri("http://www.foo.ext"));
            var uri1 = builder.ToUri();
            var uri2 = builder.ToUri();

            var query1 = HttpUtility.ParseQueryString(uri1.Query);
            var query2 = HttpUtility.ParseQueryString(uri2.Query);
            Assert.NotEqual(query1["requestId"], query2["requestId"]);
        }

        [Fact]
        public void AddQueryValuesAddsValuesToTheQuery()
        {
            var builder = new AppleDeveloperRequestUriBuilder(new Uri("http://www.foo.ext"));
            builder.AddQueryValues(new Dictionary<string, string> {
                { "x", "y" },
                { "foo", "bar" }
            });
            var uri = builder.ToUri();

            var query = HttpUtility.ParseQueryString(uri.Query);
            Assert.Equal("y", query["x"]);
            Assert.Equal("bar", query["foo"]);
        }
    }
}
