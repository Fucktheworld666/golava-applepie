namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    public class CertificateType
    {
        public string CertificateTypeDisplayId { get; set; }

        public string Name { get; set; }

        public Platform Platform { get; set; }

        public string PermissionType { get; set; }

        public string DistributionType { get; set; }

        public string DistributionMethod { get; set; }

        public string OwnerType { get; set; }

        public int DaysOverlap { get; set; }

        public int MaxActive { get; set; }
    }
}