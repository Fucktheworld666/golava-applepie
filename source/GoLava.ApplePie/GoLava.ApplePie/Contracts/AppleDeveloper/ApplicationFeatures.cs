using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    public class ApplicationFeatures
    {
        public bool Push { get; set; }

        public bool InAppPurchase { get; set; }

        public bool GameCenter { get; set; }

        public bool Passbook { get; set; }

        public string DataProtection { get; set; }

        public bool HomeKit { get; set; }

        public int CloudKitVersion { get; set; }

        [JsonProperty("iCloud")]
        public bool Cloud { get; set; }

        [JsonProperty("LPLF93JG7M")]
        public bool LPLF93JG7M { get; set; }

        [JsonProperty("IAD53UNK2F")]
        public bool InterAppAudio { get; set; }

        [JsonProperty("V66P55NK2I")]
        public bool PersonalVPN { get; set; }

        [JsonProperty("SKC3T5S89Y")]
        public bool AssociatedDomains { get; set; }

        [JsonProperty("APG3427HIY")]
        public bool AppGroups { get; set; }

        [JsonProperty("HK421J6T7P")]
        public bool HealthKit { get; set; }

        [JsonProperty("WC421J6T7P")]
        public bool WirelessAccessoryConfiguration { get; set; }

        [JsonProperty("SI015DKUHP")]
        public bool SiriKit { get; set; }

        [JsonProperty("OM633U5T5G")]
        public bool ApplePay { get; set; }

        [JsonProperty("HSC639VEI8")]
        public bool HotSpot { get; set; }

        [JsonProperty("NFCTRMAY17")]
        public bool NFCTagReading { get; set; }

        [JsonProperty("NWEXT04537")]
        public bool NetworkExtensions { get; set; }

        [JsonProperty("MP49FN762P")]
        public bool MultiPath { get; set; }
    }
}
