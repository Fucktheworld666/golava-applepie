using System;
using GoLava.ApplePie.Contracts.Attributes;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    [CsrfClass(CsrfClass.Certificate)]
    public class Certificate
    {
        public string Name { get; set; }

        public object DisplayName { get; set; }

        public string CertificateId { get; set; }

        public string SerialNumber { get; set; }

        public string Status { get; set; }

        public int StatusCode { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string ExpirationDateString { get; set; }

        [JsonProperty("certificatePlatform")]
        public Platform Platform { get; set; }

        public CertificateType CertificateType { get; set; }

        public bool HasAskKey { get; set; }
    }
}