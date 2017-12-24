using System;
using System.Collections.Generic;
using GoLava.ApplePie.Contracts.Attributes;

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

    public class Result<TData> : Result
    {
        [JsonDataClassProperty]
        public TData Data { get; set; }
    }
}