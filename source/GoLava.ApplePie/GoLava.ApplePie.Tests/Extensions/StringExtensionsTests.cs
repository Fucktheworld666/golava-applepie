using GoLava.ApplePie.Extensions;
using Xunit;

namespace GoLava.ApplePie.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null, "''")]
        [InlineData("", "''")]
        [InlineData("It's better to give than to receive", "It\\'s\\ better\\ to\\ give\\ than\\ to\\ receive")]
        [InlineData("foo '\"' bar", "foo\\ \\'\\\"\\'\\ bar")]
        [InlineData("foo \\ bar", "foo\\ \\\\\\ bar")]
        public void ShellEscape(string input, string expected)
        {
            var actual = input.ShellEscape();
            Assert.Equal(expected, actual);
        }
    }
}
