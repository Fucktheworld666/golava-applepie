namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// Token information needed by the two-step authentication.
    /// </summary>
    public class TwoStepToken
    {
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>The session identifier.</value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the value of scnt (what ever that means, but it is important).
        /// </summary>
        public string Scnt { get; set; }
    }
}
