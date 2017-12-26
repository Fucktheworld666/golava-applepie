using System;
using GoLava.ApplePie.Formatting;
using Xunit;

namespace GoLava.ApplePie.Tests.Formatting
{
    public class NamedFormatterTests
    {
        [Fact]
        public void FormatString()
        {
            var namedFormatter = new NamedFormatter();
            var result = namedFormatter.Format("Hello {name}!", new { name = "World" });
            Assert.Equal("Hello World!", result);
        }

        [Fact]
        public void FormatInt()
        {
            var namedFormatter = new NamedFormatter();
            var result = namedFormatter.Format("Hello {name}!", new { name = 42 });
            Assert.Equal("Hello 42!", result);
        }

        [Fact]
        public void FormatLong()
        {
            var namedFormatter = new NamedFormatter();
            var result = namedFormatter.Format("Hello {name}!", new { name = 42L });
            Assert.Equal("Hello 42!", result);
        }

        [Fact]
        public void FormatDateTime()
        {
            var namedFormatter = new NamedFormatter();
            var result = namedFormatter.Format("Hello {name}!", new { name = new DateTime(1979, 12, 20) });
            Assert.Equal("Hello 1979-12-20T00:00:00.0000000!", result);
        }

        [Fact]
        public void FormatFloat()
        {
            var namedFormatter = new NamedFormatter();
            var result = namedFormatter.Format("Hello {name}!", new { name = 42.1f });
            Assert.Equal("Hello 42.1!", result);
        }

        [Fact]
        public void FormatDouble()
        {
            var namedFormatter = new NamedFormatter();
            var result = namedFormatter.Format("Hello {name}!", new { name = 42.1d });
            Assert.Equal("Hello 42.1!", result);
        }

        [Fact]
        public void FormatMultiple()
        {
            var namedFormatter = new NamedFormatter();
            var result = namedFormatter.Format("{one} {two} {three} {two} {one}", new { 
                one = 1, two = "zwei", three = 3.3333 });
            Assert.Equal("1 zwei 3.3333 zwei 1", result);
        }
    }
}