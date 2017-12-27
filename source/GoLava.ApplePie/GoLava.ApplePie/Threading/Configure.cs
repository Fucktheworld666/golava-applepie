using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GoLava.ApplePie.Threading
{
    /// <summary>
    /// A helper to handle task synchronization more elegant.
    /// </summary>
    public struct Configure : INotifyCompletion
    {
        /// <summary>
        /// Configures an awaiter used to await for upcomping tasks and prevents
        /// the attempt to marshal the continuations back to the original context 
        /// captured.
        /// </summary>
        /// <returns>The false.</returns>
        public static Configure AwaitFalse()
        {
            return new Configure();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:GConfigure"/> instance is completed.
        /// </summary>
        /// <value><c>true</c> if is completed; otherwise, <c>false</c>.</value>
        public bool IsCompleted => SynchronizationContext.Current == null;

        /// <summary>
        /// Schedules the continuation action that's invoked when the instance completes.
        /// </summary>
        /// <param name="continuation">The action to invoke when the operation completes.</param>
        public void OnCompleted(Action continuation)
        {
            var prev = SynchronizationContext.Current;
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

        /// <summary>
        /// Gets an awaiter used to await this upcoming tasks.
        /// </summary>
        /// <returns>The awaiter.</returns>
        public Configure GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        public void GetResult()
        {
            // empty on purpose
        }
    }
}