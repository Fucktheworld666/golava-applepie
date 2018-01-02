using GoLava.ApplePie.Clients;
using GoLava.ApplePie.Contracts;
using Xunit;

namespace GoLava.ApplePie.Tests.Clients
{
    public class ClientContextTests
    {
        [Fact]
        public void IsForceFromBackendIsFalseByDefault()
        {
            var clientContext = new ClientContext();
            Assert.False(clientContext.IsForceFromBackend);
        }

        [Fact]
        public void AuthenticationIsNoneByDefault()
        {
            var clientContext = new ClientContext();
            Assert.Equal(Authentication.None, clientContext.Authentication);
        }

        [Fact]
        public void AsBackendContextSetsIsForceFromBackendToTrue()
        {
            var clientContext = new ClientContext();
            var backendContext = clientContext.AsBackendContext();
            Assert.True(backendContext.IsForceFromBackend);
        }

        [Fact]
        public void AsBackendContextReturnsSameInstanceWhenIsForceFromBackendIsAlreadyTrue()
        {
            var clientContext = new ClientContext();
            var backendContext = clientContext.AsBackendContext();
            Assert.Same(backendContext, backendContext.AsBackendContext());
        }

        [Fact]
        public void AsCacheContextSetsIsForceFromBackendToTrue()
        {
            var clientContext = new ClientContext().AsBackendContext();
            var cacheContext = clientContext.AsCacheContext();
            Assert.False(cacheContext.IsForceFromBackend);
        }

        [Fact]
        public void AsCacheContextReturnsSameInstanceWhenIsForceFromBackendIsAlreadyFalse()
        {
            var clientContext = new ClientContext();
            Assert.Same(clientContext, clientContext.AsCacheContext());
        }

        [Fact]
        public void AsBackendContextAssignsPropertiesCorrectly()
        {
            var clientContext = new ClientContext
            {
                Authentication = Authentication.TwoStepCode,
                AuthToken = new AuthToken(),
                LogonAuth = new LogonAuth(),
                Session = new Session(),
                TwoStepToken = new TwoStepToken()
            };
            var backendContext = clientContext.AsBackendContext();

            Assert.NotSame(clientContext, backendContext);
            Assert.Equal(clientContext.Authentication, backendContext.Authentication);
            Assert.Same(clientContext.AuthToken, backendContext.AuthToken);
            Assert.Same(clientContext.LogonAuth, backendContext.LogonAuth);
            Assert.Same(clientContext.Session, backendContext.Session);
            Assert.Same(clientContext.TwoStepToken, backendContext.TwoStepToken);
            Assert.Same(clientContext.CookieJar, backendContext.CookieJar);
        }

        [Fact]
        public void AsCacheContextAssignsPropertiesCorrectly()
        {
            var clientContext = new ClientContext
            {
                Authentication = Authentication.TwoStepCode,
                AuthToken = new AuthToken(),
                LogonAuth = new LogonAuth(),
                Session = new Session(),
                TwoStepToken = new TwoStepToken()
            };
            var cacheContext = clientContext.AsBackendContext().AsCacheContext();

            Assert.NotSame(clientContext, cacheContext);
            Assert.Equal(clientContext.Authentication, cacheContext.Authentication);
            Assert.Same(clientContext.AuthToken, cacheContext.AuthToken);
            Assert.Same(clientContext.LogonAuth, cacheContext.LogonAuth);
            Assert.Same(clientContext.Session, cacheContext.Session);
            Assert.Same(clientContext.TwoStepToken, cacheContext.TwoStepToken);
            Assert.Same(clientContext.CookieJar, cacheContext.CookieJar);
        }

        [Fact]
        public void TryGetValueReturnsTrueWhenValueExists()
        {
            var clientContext = new ClientContext();
            clientContext.AddValue("foo");

            Assert.True(clientContext.TryGetValue(out string s));
        }

        [Fact]
        public void TryGetValueReturnsFalseWhenValueNotExists()
        {
            var clientContext = new ClientContext();

            Assert.False(clientContext.TryGetValue(out string s));
        }

        [Fact]
        public void AddedValueCanBeGetAgain()
        {
            var clientContext = new ClientContext();
            clientContext.AddValue("foo");
            clientContext.TryGetValue(out string s);

            Assert.Equal("foo", s);
        }

        [Fact]
        public void AddedValueWithKeyCannotBeGetAgainWithoutKey()
        {
            var clientContext = new ClientContext();
            clientContext.AddValue("foo", 1);

            Assert.False(clientContext.TryGetValue(out string s));
            Assert.Null(s);
        }

        [Fact]
        public void DeleteReturnsTrueWhenValueExists()
        {
            var clientContext = new ClientContext();
            clientContext.AddValue("foo");

            Assert.True(clientContext.DeleteValue<string>());
        }

        [Fact]
        public void DeleteReturnsFalseWhenValueNotExists()
        {
            var clientContext = new ClientContext();

            Assert.False(clientContext.DeleteValue<string>());
        }

        [Fact]
        public void DeletedValueWithKeyCannotBeGetAgain()
        {
            var clientContext = new ClientContext();
            clientContext.AddValue("foo");
            clientContext.DeleteValue<string>();

            Assert.False(clientContext.TryGetValue(out string s));
            Assert.Null(s);
        }

        [Fact]
        public void AsBackendContextPreservesValues()
        {
            var clientContext = new ClientContext();
            clientContext.AddValue("foo");

            var backendContext = clientContext.AsBackendContext();
            Assert.True(backendContext.TryGetValue(out string s));
            Assert.Equal("foo", s);
        }

        [Fact]
        public void AsCacheContextPreservesValues()
        {
            var clientContext = new ClientContext();
            clientContext.AddValue("foo");

            var cacheContext = clientContext.AsBackendContext().AsCacheContext();
            Assert.True(cacheContext.TryGetValue(out string s));
            Assert.Equal("foo", s);
        }

        [Fact]
        public void ValueAddedToBackendContextIsAvailableInOriginalContext()
        {
            var clientContext = new ClientContext();
            var backendContext = clientContext.AsBackendContext();
            backendContext.AddValue("foo");

            Assert.True(clientContext.TryGetValue(out string s));
            Assert.Equal("foo", s);
        }

        [Fact]
        public void ValueAddedToCacheContextIsAvailableInOriginalContext()
        {
            var clientContext = new ClientContext();
            var cacheContext = clientContext.AsBackendContext().AsCacheContext();
            cacheContext.AddValue("foo");

            Assert.True(clientContext.TryGetValue(out string s));
            Assert.Equal("foo", s);
        }
    }
}
