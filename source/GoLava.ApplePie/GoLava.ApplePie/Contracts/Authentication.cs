namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// Authentication enumeration values.
    /// </summary>
    public enum Authentication
    {
        /// <summary>
        /// A value indicating that authentication was successful.
        /// </summary>
        Success,
        /// <summary>
        ///  A value indicating that authentication has failed.
        /// </summary>
        Failed,
        /// <summary>
        ///  A value indicating that a trusted device needs to be 
        /// selected for two-step authentication.
        /// </summary>
        TwoStepSelectTrustedDevice,
        /// <summary>
        ///  A value indicating that the authentication code has to be provided
        /// to verify the two-step authentication.
        /// </summary>
        TwoStepCode
    }
}