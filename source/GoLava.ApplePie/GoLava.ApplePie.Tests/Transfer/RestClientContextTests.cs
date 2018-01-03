using System;
using System.Net;
using System.Reflection;
using GoLava.ApplePie.Serializers;
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

        [Fact]
        public void CanBeSerialized()
        {
            var jsonSerializer = new JsonSerializer();
            var context = new RestClientContext();
            var json = jsonSerializer.Serialize(context);
            Assert.Equal("{\"CookieJar\":[]}", json);
        }

        [Fact]
        public void CanBeDeserialized()
        {
            var jsonSerializer = new JsonSerializer();
            var context = jsonSerializer.Deserialize<RestClientContext>("{\"CookieJar\":[]}");
            Assert.NotNull(context);
        }

        [Fact]
        public void CanBeSerializedWithCookies()
        {
            var timeStamp = new DateTime(1979, 12, 20, 1, 2, 3, DateTimeKind.Utc);

            var jsonSerializer = new JsonSerializer();
            var context = new RestClientContext();
            context.CookieJar.Add(new Uri("http://www.foo.ext"), 
                SetTimestamp(new Cookie("foo1", "bar", "/", ".foo.ext"), timeStamp));
            context.CookieJar.Add(new Uri("https://www.foo.ext"),
                SetTimestamp(new Cookie("foo2", "bar", "/", ".foo.ext") { Secure = true, Comment = "hello world", HttpOnly = true }, timeStamp));
            var json = jsonSerializer.Serialize(context);
            Assert.Equal("{\"CookieJar\":[{\"Domain\":\".foo.ext\",\"Cookies\":[{\"Comment\":\"\",\"HttpOnly\":false,\"Discard\":false,\"Domain\":\".foo.ext\",\"Expired\":false,\"Expires\":\"0001-01-01T00:00:00\",\"Name\":\"foo1\",\"Path\":\"/\",\"Port\":\"\",\"Secure\":false,\"TimeStamp\":\"1979-12-20T01:02:03Z\",\"Value\":\"bar\",\"Version\":0}]},{\"Domain\":\".foo.ext\",\"Cookies\":[{\"Comment\":\"hello world\",\"HttpOnly\":true,\"Discard\":false,\"Domain\":\".foo.ext\",\"Expired\":false,\"Expires\":\"0001-01-01T00:00:00\",\"Name\":\"foo2\",\"Path\":\"/\",\"Port\":\"\",\"Secure\":true,\"TimeStamp\":\"1979-12-20T01:02:03Z\",\"Value\":\"bar\",\"Version\":0}]}]}", json);
        }

        [Fact]
        public void CanBeDeserializedWithCookies()
        {
            var timeStamp = new DateTime(1979, 12, 20, 1, 2, 3, DateTimeKind.Utc);

            var jsonSerializer = new JsonSerializer();
            var context = jsonSerializer.Deserialize<RestClientContext>("{\"CookieJar\":[{\"Domain\":\".foo.ext\",\"Cookies\":[{\"Comment\":\"hello world\",\"CommentUri\":null,\"HttpOnly\":true,\"Discard\":false,\"Domain\":\".foo.ext\",\"Expired\":false,\"Expires\":\"0001-01-01T00:00:00\",\"Name\":\"foo2\",\"Path\":\"/\",\"Port\":\"\",\"Secure\":true,\"TimeStamp\":\"1979-12-20T01:02:03Z\",\"Value\":\"bar\",\"Version\":0},{\"Comment\":\"\",\"CommentUri\":null,\"HttpOnly\":false,\"Discard\":false,\"Domain\":\".foo.ext\",\"Expired\":false,\"Expires\":\"0001-01-01T00:00:00\",\"Name\":\"foo1\",\"Path\":\"/\",\"Port\":\"\",\"Secure\":false,\"TimeStamp\":\"1979-12-20T01:02:03Z\",\"Value\":\"bar\",\"Version\":0}]}]}");
            Assert.NotNull(context);

            var cookies = context.CookieJar.GetCookies(new Uri("https://www.foo.ext"));
            Assert.Equal(2, cookies.Count);

            Assert.Equal("foo1", cookies[0].Name);
            Assert.Equal("bar", cookies[0].Value);
            Assert.Equal("", cookies[0].Comment);
            Assert.False(cookies[0].HttpOnly);
            Assert.False(cookies[0].Secure);

            Assert.Equal("foo2", cookies[1].Name);
            Assert.Equal("bar", cookies[1].Value);
            Assert.Equal("hello world", cookies[1].Comment);
            Assert.True(cookies[1].HttpOnly);
            Assert.True(cookies[1].Secure);
        }

        private Cookie SetTimestamp(Cookie cookie, DateTime timeStamp)
        {
            var timeStampField = typeof(Cookie).GetField("m_timeStamp", BindingFlags.NonPublic | BindingFlags.Instance);
            timeStampField.SetValue(cookie, timeStamp);
            return cookie;
        }
    }
}