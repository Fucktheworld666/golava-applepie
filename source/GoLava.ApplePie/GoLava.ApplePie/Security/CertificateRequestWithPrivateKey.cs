namespace GoLava.ApplePie.Security
{
    /// <summary>
    /// a Certificate request with private key.
    /// </summary>
    public class CertificateRequestWithPrivateKey
    {
        /// <summary>
        /// Gets or sets the certificate request in PEM format.
        /// </summary>
        /// <value>The certificate request.</value>
        public string CertificateRequest { get; set; }

        /// <summary>
        /// Gets or sets the private key in PEM format.
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the public key in PEM format.
        /// </summary>
        public string PublicKey { get; set; }
    }
}