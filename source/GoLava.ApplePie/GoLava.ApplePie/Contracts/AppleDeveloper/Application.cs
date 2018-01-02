using GoLava.ApplePie.Contracts.Attributes;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    /// <summary>
    /// The application contract.
    /// </summary>
    [CsrfClass(CsrfClass.Application), JsonDataProperty("appId")]
    public class Application
    {
        /// <summary>
        /// Gets or sets the short identifier of the application.
        /// </summary>
        [JsonProperty("appIdId")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the platform of the application.
        /// </summary>
        [JsonProperty("appIdPlatform")]
        public Platform Platform { get; set; }

        /// <summary>
        /// Gets or sets the prefix of the application.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the long identifier of the application.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets a value whether the application is a wildcard application.
        /// </summary>
        public bool IsWildCard { get; set; }

        /// <summary>
        /// Gets or sets a value whether the application is a duplicate.
        /// </summary>
        public bool IsDuplicate { get; set; }

        /// <summary>
        /// Gets or sets a value whether the application can be edited.
        /// </summary>
        public bool? CanEdit { get; set; }

        /// <summary>
        /// Gets or sets a value whether the application can be deleted.
        /// </summary>
        public bool? CanDelete { get; set; }

        /// <summary>
        /// Gets or sets the team identifier of the application.
        /// </summary>
        [JsonIgnore]
        public string TeamId { get; internal set; }
    }
}