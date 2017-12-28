using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// Holds information about a particular user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [JsonProperty(PropertyName = "prsId")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
