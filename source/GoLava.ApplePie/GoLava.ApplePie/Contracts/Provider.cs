using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts
{
    public class Provider
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [JsonProperty(PropertyName = "providerId")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a list of content types.
        /// </summary>
        /// <value>The content types.</value>
        public List<string> ContentTypes { get; set; }
    }
}