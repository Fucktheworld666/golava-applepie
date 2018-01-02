using System;
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
    }
}
