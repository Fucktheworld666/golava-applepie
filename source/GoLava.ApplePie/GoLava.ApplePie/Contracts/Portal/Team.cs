using System;
using GoLava.ApplePie.Contracts.Converters;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.Portal
{
    public class Team
    {
        public string TeamId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public Address Address { get; set; }

        public string EntityType { get; set; }

        public Person Agent { get; set; }

        public Program Program { get; set; }

        public string UserRole { get; set; }

        public string TeamMemberId { get; set; }

        public int AdminCount { get; set; }

        public int MemberCount { get; set; }

        public int ServerCount { get; set; }

        [JsonConverter(typeof(MillisecondsEpochConverter))]
        public DateTime NextDeviceResetDate { get; set; }
    }
}