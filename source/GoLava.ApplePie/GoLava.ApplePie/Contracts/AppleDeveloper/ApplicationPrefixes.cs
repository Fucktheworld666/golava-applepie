using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    /// <summary>
    /// Application prefixes contract.
    /// </summary>
    public class ApplicationPrefixes : Result
    {
        /// <summary>
        /// Gets or sets the application identifier prefixes.
        /// </summary>
        public List<string> AppIdPrefixes { get; set; }
    }
}