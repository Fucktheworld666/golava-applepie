using System;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.Portal
{
    public class MemberShip
    {
        public string MembershipId { get; set; }

        public string MembershipProductId { get; set; }

        public string Status { get; set; }

        public bool? InIosDeviceResetWindow { get; set; }

        public bool? InMacDeviceResetWindow { get; set; }

        public bool? InRenewalWindow { get; set; }

        public string DateStart { get; set; }

        public string DateExpire { get; set; }

        public string Platform { get; set; }

        public string Name { get; set; }
    }
}