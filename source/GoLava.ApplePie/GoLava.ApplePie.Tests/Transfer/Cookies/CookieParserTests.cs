using System;
using GoLava.ApplePie.Transfer.Cookies;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer.Cookies
{
    public class CookieParserTests
    {
        [Fact]
        public void ParseNameAndValueCorrectly()
        {
            var cookieParser = new CookieParser();
            var cookie = cookieParser.Parse("foo=bar");
            Assert.Equal("foo", cookie.Name);
            Assert.Equal("bar", cookie.Value);
        }

        [Fact]
        public void ParseNameAndValueAndHttpOnlyCorrectly()
        {
            var cookieParser = new CookieParser();
            var cookie = cookieParser.Parse("foo=bar; httponly");
            Assert.Equal("foo", cookie.Name);
            Assert.Equal("bar", cookie.Value);
            Assert.True(cookie.HttpOnly, "httponly mismatch");
        }

        [Fact]
        public void ParseNameAndValueAndSecureCorrectly()
        {
            var cookieParser = new CookieParser();
            var cookie = cookieParser.Parse("foo=bar; secure");
            Assert.Equal("foo", cookie.Name);
            Assert.Equal("bar", cookie.Value);
            Assert.True(cookie.Secure, "secure mismatch");
        }

        [Fact]
        public void ParseNameAndValueAndPathCorrectly()
        {
            var cookieParser = new CookieParser();
            var cookie = cookieParser.Parse("foo=bar; path=/");
            Assert.Equal("foo", cookie.Name);
            Assert.Equal("bar", cookie.Value);
            Assert.Equal("/", cookie.Path);
        }

        [Fact]
        public void ParseNameAndValueAndDomainCorrectly()
        {
            var cookieParser = new CookieParser();
            var cookie = cookieParser.Parse("foo=bar; domain=domain.ext");
            Assert.Equal("foo", cookie.Name);
            Assert.Equal("bar", cookie.Value);
            Assert.Equal("domain.ext", cookie.Domain);
        }

        [Fact]
        public void ParseNameAndValueAndExpiresCorrectly()
        {
            var cookieParser = new CookieParser();
            var cookie = cookieParser.Parse("foo=bar; Expires=Wed, 21 Oct 2015 07:28:00 GMT");
            Assert.Equal("foo", cookie.Name);
            Assert.Equal("bar", cookie.Value);
            Assert.Equal(new DateTime(2015, 10, 21, 7, 28, 0, DateTimeKind.Utc), cookie.Expires);
        }

        [Fact]
        public void ParseNameAndValueAndPathAndDomainExpiresCorrectly()
        {
            var cookieParser = new CookieParser();
            var cookie = cookieParser.Parse("qwerty=219ffwef9w0f; Domain=somecompany.co.uk; Path=/; Expires=Fri, 30 Aug 2019 00:00:00 GMT");
            Assert.Equal("qwerty", cookie.Name);
            Assert.Equal("219ffwef9w0f", cookie.Value);
            Assert.Equal("somecompany.co.uk", cookie.Domain);
            Assert.Equal(new DateTime(2019, 8, 30, 0, 0, 0, DateTimeKind.Utc), cookie.Expires);
        }
    }
}