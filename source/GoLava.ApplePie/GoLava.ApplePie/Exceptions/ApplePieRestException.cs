using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Transfer;

namespace GoLava.ApplePie.Exceptions
{
    /// <summary>
    /// A ApplePie Rest exception.
    /// </summary>
    public class ApplePieRestException : ApplePieException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ApplePieRestException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        /// <param name="response">The rest response received when the exception occured.</param>
        public ApplePieRestException(string message, RestResponse response, ErrorCode errorCode)
            : base(message)
        {
            this.Response = response;
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the rest response.
        /// </summary>
        public RestResponse Response { get; }

        /// <summary>
        /// Gets the error code that was extracted from the rest call.
        /// </summary>
        public ErrorCode ErrorCode { get; }
    }

    /// <summary>
    /// A ApplePie Rest exception.
    /// </summary>
    public class ApplePieRestException<T> : ApplePieRestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ApplePieRestException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        /// <param name="response">The rest response received when the exception occured.</param>
        public ApplePieRestException(string message, RestResponse<T> response, ErrorCode errorCode)
            : base(message, response, errorCode) { }

        /// <summary>
        /// Gets the rest response.
        /// </summary>
        public new RestResponse<T> Response => base.Response as RestResponse<T>;
    }
}