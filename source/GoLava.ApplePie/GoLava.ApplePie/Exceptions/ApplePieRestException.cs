using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Transfer;

namespace GoLava.ApplePie.Exceptions
{
    /// <summary>
    /// A ApplePie Rest exception.
    /// </summary>
    public class ApplePieRestException<T> : ApplePieException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ApplePieRestException`1"/> class.
        /// </summary>
        /// <param name="message">The message that describes the exception.</param>
        /// <param name="response">The rest response received when the exception occured.</param>
        public ApplePieRestException(string message, RestResponse<T> response, ErrorCode errorCode)
            : base(message)
        {
            this.Response = response;
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the rest response.
        /// </summary>
        public RestResponse<T> Response { get; }

        /// <summary>
        /// Gets the error code that was extracted from the rest call.
        /// </summary>
        public ErrorCode ErrorCode { get; }
    }
}