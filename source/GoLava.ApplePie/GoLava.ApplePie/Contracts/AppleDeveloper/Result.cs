using System;
using System.Collections.Generic;
using GoLava.ApplePie.Contracts.Attributes;
using GoLava.ApplePie.Contracts.Converters;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    /// <summary>
    /// A result contract.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        public DateTime CreationTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the result code.
        /// </summary>
        public int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the request URL.
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// Gets or sets the response identifier.
        /// </summary>
        public string ResponseId { get; set; }

        /// <summary>
        /// Gets or sets the protocol version.
        /// </summary>
        public string ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the user string.
        /// </summary>
        public string UserString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user that made the request is a team admin.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user that made the request is a team member.
        /// </summary>
        public bool IsMember { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user that made the request is a team agent.
        /// </summary>
        public bool IsAgent { get; set; }

        /// <summary>
        /// Gets or sets a list of validation messages.
        /// </summary>
        public List<ValidationMessage> ValidationMessages { get; set; }

        /// <summary>
        /// Gets or sets the next device reset date.
        /// </summary>
        [JsonConverter(typeof(MillisecondsEpochConverter))]
        public DateTime? NextDeviceResetDate { get; set; }
    }

    /// <summary>
    /// A result contract with data.
    /// </summary>
    public class Result<TData> : Result
    {
        /// <summary>
        /// Gets or sets the data of the result.
        /// </summary>
        [JsonDataClassProperty]
        public TData Data { get; set; }
    }
}