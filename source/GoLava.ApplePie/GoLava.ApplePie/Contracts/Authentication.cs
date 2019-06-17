namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// Authentication enumeration values.
    /// </summary>
    public enum Authentication
    {
        /// <summary>
        /// A value indicating that no authentication 
        /// </summary>
        None,
        /// <summary>
        /// A value indicating that authentication was successful.
        /// </summary>
        Success,
        /// <summary>
        ///  A value indicating that a trusted device needs to be 
        /// selected for two-step authentication.
        /// </summary>
        TwoStepSelectTrustedDevice,
        /// <summary>
        ///  A value indicating that the authentication code has to be provided
        /// to verify the two-step authentication.
        /// </summary>
        TwoStepCode,
        /// <summary>
        ///  A value indicating that authentication has failed with invalid credentials.
        /// </summary>
        FailedWithInvalidCredentials,
        /// <summary>
        ///  A value indicating that authentication has failed unexpected.
        /// </summary>
        FailedUnexpected,
        /// <summary>
        /// A value indicating that authentication has failed because no trusted device was found.
        /// </summary>
        FailedNoTrustedDeviceFound,
        /// <summary>
        /// A value indicating that the apple id and privacy statement needs to be acknowledged.
        /// </summary>
        FailedNeedsToAcknowledgeAppleIdAndPrivacyStatement
    }

    public static class AuthenticationExtensions
    {
        public static bool HasFailed(this Authentication authentication)
        {
            return authentication == Authentication.FailedUnexpected
                || authentication == Authentication.FailedNoTrustedDeviceFound
                || authentication == Authentication.FailedWithInvalidCredentials
			    || authentication == Authentication.FailedNeedsToAcknowledgeAppleIdAndPrivacyStatement;
        }
    }
}