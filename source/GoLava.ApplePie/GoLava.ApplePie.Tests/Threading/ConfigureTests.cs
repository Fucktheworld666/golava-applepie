using System.Threading;
using System.Threading.Tasks;
using GoLava.ApplePie.Threading;
using Xunit;

namespace GoLava.ApplePie.Tests.Threading
{
    public class ConfigureTests
    {
        [Fact]
        public async Task SynchronizationContextIsNullAfterAwait()
        {
            await Configure.AwaitFalse();
            Assert.Null(SynchronizationContext.Current);
        }

        [Fact]
        public async Task SynchronizationContextIsNotNullBeforeAwait()
        {
            Assert.NotNull(SynchronizationContext.Current);
            await Configure.AwaitFalse();
        }

        [Fact]
        public async Task IsCompletedIsTrueAfterAwait()
        {
            var configure = Configure.AwaitFalse();
            await configure;
            Assert.True(configure.IsCompleted);
        }

        [Fact]
        public async Task IsCompletedIsFalseBeforeAwait()
        {
            var configure = Configure.AwaitFalse();
            Assert.False(configure.IsCompleted);
            await configure;
        }

        [Fact]
        public async Task SynchronizationContextGetsRestored()
        {
            var context = SynchronizationContext.Current;
            await this.ConfigureAwaitFalseAsync();
            Assert.Same(context, SynchronizationContext.Current);
        }

        private async Task ConfigureAwaitFalseAsync()
        {
            await Configure.AwaitFalse();
        }
    }
}