namespace GoLava.ApplePie.Contracts
{
    public class TrustedDevice
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public bool Trusted { get; set; }

        public string Status { get; set; }

        public string LiveStatus { get; set; }

        public string LastTwoDigits { get; set; }

        public string ImageLocation { get; set; }

        public string ImageLocation2x { get; set; }

        public string ImageLocation3x { get; set; }

        public long UpdateDate { get; set; }

        public string NumberWithAreaCodeCountryDialingCode { get; set; }

        public bool ThisDevice { get; set; }

        public bool Online { get; set; }

        public bool VirtualDevice { get; set; }

        public bool Device { get; set; }
    }
}