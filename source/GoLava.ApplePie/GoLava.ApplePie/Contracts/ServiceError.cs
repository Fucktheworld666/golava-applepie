namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// Error returned by a service call.
    /// </summary>
    public class ServiceError
    {
        /// <summary>
        /// Gets or sets the code of the error.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the message of the error.
        /// </summary>
        public string Message { get; set; }
    }
}