using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GoLava.ApplePie.Serializers;
using GoLava.ApplePie.Transfer;
using RichardSzalay.MockHttp;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer
{
    public class RestClientTests
    {
        [Fact]
        public async Task ContentIsHandeledCorrectly()
        {
            var json = "{'foo' : 'Bar'}";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect("http://domain.ext/")
                .Respond("application/json", json);

            var restClient = new RestClient(JsonSerializer.Create(), mockHttp);
            var context = new RestClientContext();
            var request = RestRequest.Get(new RestUri("http://domain.ext/"));
            var response = await restClient.SendAsync<Contract>(context, request);

            Assert.True(response.IsSuccess, "IsSuccess is false");
            Assert.Equal(RestContentType.Json, response.ContentType);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(json, response.RawContent.ToString());
            Assert.Equal(response.Content.Foo, "Bar");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task UserAgentIsSetCorrectly()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect("http://domain.ext/")
                .WithHeaders("user-agent", "GoLava/1.0")
                .Respond("application/json", "{'foo' : 'Bar'}");

            var restClient = new RestClient(JsonSerializer.Create(), mockHttp);
            var context = new RestClientContext();
            var request = RestRequest.Get(new RestUri("http://domain.ext/"));
            var response = await restClient.SendAsync<Contract>(context, request);

            Assert.True(response.IsSuccess);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task ConnectionKeepAliveSetCorrectly()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect("http://domain.ext/")
                .WithHeaders("connection", "keep-alive")
                .Respond("application/json", "{'foo' : 'Bar'}");

            var restClient = new RestClient(JsonSerializer.Create(), mockHttp);
            var context = new RestClientContext();
            var request = RestRequest.Get(new RestUri("http://domain.ext/"));
            var response = await restClient.SendAsync<Contract>(context, request);

            Assert.True(response.IsSuccess);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task HostIsSetCorrectly()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect("http://domain.ext/")
                .WithHeaders("host", "domain.ext")
                .Respond("application/json", "{'foo' : 'Bar'}");

            var restClient = new RestClient(JsonSerializer.Create(), mockHttp);
            var context = new RestClientContext();
            var request = RestRequest.Get(new RestUri("http://domain.ext/"));
            var response = await restClient.SendAsync<Contract>(context, request);

            Assert.True(response.IsSuccess);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task CookieIsSetCorrectly()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect("http://domain.ext/")
                .WithHeaders("cookie", "foo=bar; hello=world")
                .Respond("application/json", "{'foo' : 'Bar'}");

            var restClient = new RestClient(JsonSerializer.Create(), mockHttp);
            var context = new RestClientContext();
            context.CookieJar.Add(new Uri("http://domain.ext/"), new Cookie("foo", "bar"));
            context.CookieJar.Add(new Uri("http://domain.ext/"), new Cookie("hello", "world"));
            var request = RestRequest.Get(new RestUri("http://domain.ext/"));
            var response = await restClient.SendAsync<Contract>(context, request);

            Assert.True(response.IsSuccess);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task CookiesAreHandeledCorrectly()
        {
            var res = new HttpResponseMessage(HttpStatusCode.OK);
            res.Headers.TryAddWithoutValidation("set-cookie", "hello=world");

            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect("http://domain.ext/")
                .Respond(req => res);
            var restClient = new RestClient(JsonSerializer.Create(), mockHttp);
            var context = new RestClientContext();
            var request = RestRequest.Get(new RestUri("http://domain.ext/"));
            var response = await restClient.SendAsync<Contract>(context, request);

            var cookies = context.CookieJar.GetCookies(new RestUri("http://domain.ext/"));
            Assert.Equal(1, cookies.Count);
            Assert.Equal("hello", cookies[0].Name);
            Assert.Equal("world", cookies[0].Value);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        public class Contract
        {
            public string Foo { get; set; }
        }
    }
}
