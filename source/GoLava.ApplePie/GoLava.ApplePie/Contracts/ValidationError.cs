namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// Error returned by a validation call.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Gets or sets the code of the error.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the message of the error.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the title of the error.
        /// </summary>
        public string Title { get; set; }
    }
}