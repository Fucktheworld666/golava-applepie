using System;
using GoLava.ApplePie.Contracts.Converters;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.Portal
{
    public class Program
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public bool AutoRenew { get; set; }

        [JsonConverter(typeof(MillisecondsEpochConverter))]
        public DateTime DateExpires { get; set; }

        public string AutoRenewPrice { get; set; }
    }
}