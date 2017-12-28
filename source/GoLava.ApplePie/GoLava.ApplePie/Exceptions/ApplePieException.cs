using System;

namespace GoLava.ApplePie.Exceptions
{
    /// <summary>
    /// A ApplePie exception.
    /// </summary>
    public class ApplePieException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ApplePieException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        public ApplePieException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ApplePieException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        /// <param name="innerException">An inner exception that was raised 
        /// previously to this <see cref="T:ApplePieException"/> instance.</param>
        public ApplePieException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}