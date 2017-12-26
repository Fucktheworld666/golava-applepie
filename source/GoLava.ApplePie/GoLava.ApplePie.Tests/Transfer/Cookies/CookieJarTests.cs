using System;
using System.Linq;
using System.Net;
using GoLava.ApplePie.Transfer.Cookies;
using Xunit;

namespace GoLava.ApplePie.Tests.Transfer.Cookies
{
    public class CookieJarTests
    {
        [Fact]
        public void AddCookieThatDoesNotMatchDomainWillThrowException()
        {
            var cookieJar = new CookieJar();
            Assert.Throws<CookieException>(() => cookieJar.Add(new Uri("http://bar.ext"), new Cookie { Domain = "foo.ext" }));
        }

        [Fact]
        public void AddCookieThatDoesNotMatchSubDomainWillThrowException()
        {
            var cookieJar = new CookieJar();
            Assert.Throws<CookieException>(() => cookieJar.Add(new Uri("http://bar.ext"), new Cookie { Domain = "foo.bar.ext" }));
        }

        [Fact]
        public void AddCookieWithSameDomainIsSuccessful()
        {
            var cookieJar = new CookieJar();
            cookieJar.Add(new Uri("http://bar.ext"), new Cookie { Domain = "bar.ext" });
        }

        [Fact]
        public void AddCookieWithSubDomainIsSuccessful()
        {
            var cookieJar = new CookieJar();
            cookieJar.Add(new Uri("http://foo.bar.ext"), new Cookie { Domain = "bar.ext" });
        }

        [Fact]
        public void AddCookieWithSubSubDomainIsSuccessful()
        {
            var cookieJar = new CookieJar();
            cookieJar.Add(new Uri("http://baz.foo.bar.ext"), new Cookie { Domain = "bar.ext" });
        }

        [Fact]
        public void GetCookiesWillReturnAllMatchingCookies()
        {
            var cookieJar = new CookieJar();
            cookieJar.Add(new Uri("http://bar.ext"), new Cookie { Name = "a", Domain = "bar.ext" });
            cookieJar.Add(new Uri("http://foo.bar.ext"), new Cookie { Name = "b", Domain = "bar.ext" });
            cookieJar.Add(new Uri("http://foo.ext"), new Cookie { Name = "c", Domain = "foo.ext" });

            var cookies = cookieJar.GetCookies(new Uri("http://bar.ext"));
            Assert.Equal(2, cookies.Count);
            Assert.True(cookies.Cast<Cookie>().All(c => c.Domain == "bar.ext"));
        }

        [Fact]
        public void ClearRemovesAllCookies()
        {
            var cookieJar = new CookieJar();
            cookieJar.Add(new Uri("http://bar.ext"), new Cookie { Name = "a", Domain = "bar.ext" });
            cookieJar.Add(new Uri("http://foo.bar.ext"), new Cookie { Name = "b", Domain = "bar.ext" });
            cookieJar.Add(new Uri("http://foo.ext"), new Cookie { Name = "c", Domain = "foo.ext" });
            cookieJar.Clear();

            var cookies = cookieJar.GetCookies(new Uri("http://bar.ext"));
            Assert.Equal(0, cookies.Count);
        }

        [Fact]
        public void CookieValuesAreOverwritten()
        {
            var cookieJar = new CookieJar();
            cookieJar.Add(new Uri("http://bar.ext"), new Cookie { Name = "a", Value = "x", Domain = "bar.ext" });
            cookieJar.Add(new Uri("http://foo.bar.ext"), new Cookie { Name = "a", Value = "y", Domain = "bar.ext" });

            var cookies = cookieJar.GetCookies(new Uri("http://bar.ext"));
            Assert.Equal(1, cookies.Count);
            Assert.Equal("y", cookies[0].Value);
        }

        [Fact]
        public void GetRequestHeaderValueReturnsCorrectValue()
        {
            var cookieJar = new CookieJar();
            cookieJar.Add(new Uri("http://bar.ext"), new Cookie { Name = "a", Value = "x", Domain = "bar.ext" });
            cookieJar.Add(new Uri("http://foo.bar.ext"), new Cookie { Name = "b", Value = "y", Domain = "bar.ext" });

            var header = cookieJar.GetRequestHeaderValue(new Uri("http://bar.ext"));
            Assert.Equal("a=x; b=y", header);
        }
    }
}
