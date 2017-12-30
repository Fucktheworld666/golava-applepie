using GoLava.ApplePie.Contracts.Attributes;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    [CsrfClass(CsrfClass.Application), JsonDataProperty("appId")]
    public class Application
    {
        [JsonProperty("appIdId")]
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("appIdPlatform")]
        public string Platform { get; set; }

        public string Prefix { get; set; }

        public string Identifier { get; set; }

        public bool IsWildCard { get; set; }

        public bool IsDuplicate { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
    }
}