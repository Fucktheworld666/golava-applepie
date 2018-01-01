﻿using GoLava.ApplePie.Contracts.Attributes;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    [CsrfClass(CsrfClass.Device), JsonDataProperty]
    public class Device
    {
        public string DeviceId { get; set; }

        public string Name { get; set; }

        public string DeviceNumber { get; set; }

        public string DevicePlatform { get; set; }

        public string Status { get; set; }

        public string DeviceClass { get; set; }

        public string Model { get; set; }

        [JsonIgnore]
        public string TeamId { get; set; }
    }
}