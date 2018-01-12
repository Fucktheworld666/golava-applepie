using GoLava.ApplePie.Contracts.Attributes;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    /// <summary>
    /// Submit certificate request to be sent for new certificates to be issued. 
    /// </summary>
    [CsrfClass(CsrfClass.Certificate)]
    public class CertificateSigningRequest
    {
        /// <summary>
        /// Gets or sets the type of the certificate to be issued.
        /// </summary>
        /// <value>The type.</value>
        public CertificateTypeDisplayId Type { get; set; }

        /// <summary>
        /// Gets or sets the base64 content of the certificate request.
        /// </summary>
        /// <value>The content.</value>
        [JsonProperty("csrContent")]
        public string Content { get; set; }

        [JsonProperty("appIdId")]
        public string ApplicationId { get; set; }

        public string SpecialIdentifierDisplayId
        {
            get => this.ApplicationId;
            set => this.ApplicationId = value;
        }
    }
}
