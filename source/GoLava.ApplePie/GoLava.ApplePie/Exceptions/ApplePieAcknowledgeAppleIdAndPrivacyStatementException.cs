using System;
namespace GoLava.ApplePie.Exceptions
{
	/// <summary>
    /// A ApplePie acknowledge apple identifier and privacy statement exception.
    /// </summary>
	public class ApplePieAcknowledgeAppleIdAndPrivacyStatementException : ApplePieException
    {
		/// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:GoLava.ApplePie.Exceptions.ApplePieAcknowledgeAppleIdAndPrivacyStatementException"/> class.
        /// </summary>
        /// <param name="innerException">Inner exception.</param>
        public ApplePieAcknowledgeAppleIdAndPrivacyStatementException(Exception innerException)
			: base("Need to acknowledge to Apple's Apple ID and Privacy statement. Please manually log into https://appleid.apple.com (or https://itunesconnect.apple.com) to acknowledge the statement.", innerException)
        {
        }
    }
}
