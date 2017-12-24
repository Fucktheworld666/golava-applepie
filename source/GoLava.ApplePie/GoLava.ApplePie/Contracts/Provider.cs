using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts
{
    public class Provider
    {
        [JsonProperty(PropertyName = "providerId")]
        public string Id { get; set; }

        public string Name { get; set; }

        public List<string> ContentTypes { get; set; }
    }
}