using System;
using System.Collections.Generic;
using GoLava.ApplePie.Contracts.Attributes;
using GoLava.ApplePie.Contracts.Converters;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
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

        public bool IsAdmin { get; set; }

        public bool IsMember { get; set; }

        public bool IsAgent { get; set; }

        public List<ValidationMessage> ValidationMessages { get; set; }

        [JsonConverter(typeof(MillisecondsEpochConverter))]
        public DateTime? NextDeviceResetDate { get; set; }
    }

    public class Result<TData> : Result
    {
        [JsonDataClassProperty]
        public TData Data { get; set; }
    }
}