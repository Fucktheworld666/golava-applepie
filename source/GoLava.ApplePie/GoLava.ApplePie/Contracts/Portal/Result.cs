using System;
namespace GoLava.ApplePie.Contracts.Portal
{
    public class Result
    {
        public DateTime CreationTimestamp { get; set; }

        public int ResultCode { get; set; }

        public string RequestId { get; set; }

        public string RequestUrl { get; set; }

        public string ResponseId { get; set; }

        public string ProtocolVersion { get; set; }

        public string UserString { get; set; }
    }
}