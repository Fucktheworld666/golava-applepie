using System;
using GoLava.ApplePie.Contracts.Attributes;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    [CsrfClass(CsrfClass.Certificate), JsonDataProperty("certRequest")]
    public class CertificateRequest
    {
        public string CertRequestId { get; set; }

        public string Name { get; set; }

        public int StatusCode { get; set; }

        public string StatusString { get; set; }

        [JsonProperty("csrPlatform")]
        public Platform Platform { get; set; }

        public string DateRequestedString { get; set; }

        public DateTime DateRequested { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string ExpirationDateString { get; set; }

        public string OwnerType { get; set; }

        public string OwnerName { get; set; }

        public string OwnerId { get; set; }

        public bool CanDownload { get; set; }

        public bool CanRevoke { get; set; }

        public string CertificateId { get; set; }

        public int CertificateStatusCode { get; set; }

        public int CertRequestStatusCode { get; set; }

        public string CertificateTypeDisplayId { get; set; }

        [JsonProperty("serialNum")]
        public string SerialNumber { get; set; }

        public string TypeString { get; set; }

        public CertificateType CertificateType { get; set; }

        public Certificate Certificate { get; set; }

        /// <summary>
        /// Gets or sets the team identifier.
        /// </summary>
        [JsonIgnore]
        public string TeamId { get; internal set; }
    }
}