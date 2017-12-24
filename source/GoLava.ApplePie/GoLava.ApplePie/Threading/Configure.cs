using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GoLava.ApplePie.Threading
{
    public struct Configure : INotifyCompletion
    {
        public static Configure AwaitFalse()
        {
            return new Configure();
        }

        public bool IsCompleted => SynchronizationContext.Current == null;

        public void OnCompleted(Action continuation)
        {
            var prev = SynchronizationContext.Current;
            if (prev == null)
                return;

            try
            {
                SynchronizationContext.SetSynchronizationContext(null);
                continuation();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(prev);
            }
        }

        public Configure GetAwaiter()
        {
            return this;
        }

        public void GetResult()
        {
            // empty on purpose
        }
    }
}
