using System;
using System.Net.Http;
using GoLava.ApplePie.Transfer.Handlers;
using Xunit;

namespace GoLava.ApplePie.Tests.Handlers
{
    public class HttpMessageHandlerExtensionsTests
    {
        [Fact]
        public void PassingNullAsOuterHandlerWillRaiseArgumentNullException()
        {
            DelegatingHandler outerHandler = null;
            var exception = Assert.Throws<ArgumentNullException>(() => new HttpClientHandler().DecorateWith(outerHandler));
            Assert.Equal(nameof(outerHandler), exception.ParamName);
        }

        [Fact]
        public void InnerHandlerAreSameInstance()
        {
            var innerHandler = new HttpClientHandler();
            var outerHandler = new TestDelegatingHandler();

            innerHandler.DecorateWith(outerHandler);

            Assert.Same(innerHandler, outerHandler.InnerHandler);
        }

        [Fact]
        public void OuterHandlerAndResultAreSameInstance()
        {
            var innerHandler = new HttpClientHandler();
            var outerHandler = new TestDelegatingHandler();

            var result = innerHandler.DecorateWith(outerHandler);

            Assert.Same(outerHandler, result);
        }

        public class TestDelegatingHandler : DelegatingHandler { }
    }
}
