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
        public ApplePieRestException(string message, RestResponse<T> response)
            : base(message)
        {
            this.Response = response;
        }

        /// <summary>
        /// Gets the rest response.
        /// </summary>
        public RestResponse<T> Response { get; }
    }
}