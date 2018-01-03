using System;

namespace GoLava.ApplePie.Exceptions
{
    /// <summary>
    /// A ApplePie credentials exception.
    /// </summary>
    public class ApplePieCredentialsException : ApplePieException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ApplePieCredentialsException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        /// <param name="innerException">An inner exception that was raised 
        /// previously to this <see cref="T:ApplePieCredentialsException"/> instance.</param>
        public ApplePieCredentialsException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
