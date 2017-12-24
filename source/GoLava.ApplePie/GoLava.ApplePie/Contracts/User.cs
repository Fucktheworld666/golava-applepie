using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts
{
    public class User
    {
        [JsonProperty(PropertyName = "prsId")]
        public string Id { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }
    }
}
