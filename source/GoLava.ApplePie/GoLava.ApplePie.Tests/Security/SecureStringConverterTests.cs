using System.Security;
using GoLava.ApplePie.Security;
using Xunit;

namespace GoLava.ApplePie.Tests.Security
{
    public class SecureStringConverterTests
    {
        [Fact]
        public void ConvertSecureStringToPlainString()
        {
            const string expected = "secret";
                
            var secureStringConverter = new SecureStringConverter();
            var actual = secureStringConverter.ConvertSecureStringToPlainString(CreateSecureString(expected));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertSecureStringToPlainCharArray()
        {
            const string expected = "secret";

            var secureStringConverter = new SecureStringConverter();
            var actual = secureStringConverter.ConvertSecureStringToPlainCharArray(CreateSecureString(expected));

            Assert.Equal(expected.ToCharArray(), actual);
        }


        private SecureString CreateSecureString(string plainString)
        {
            var secureString = new SecureString();
            foreach (var c in plainString)
                secureString.AppendChar(c);
            return secureString;
        }
    }
}