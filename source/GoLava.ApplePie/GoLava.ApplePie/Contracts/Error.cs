using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// An error contract that handles multiple error variations send my apple.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Gets or sets a list of <see cref="T:ServiceError"/> instances.
        /// </summary>
        public List<ServiceError> ServiceErrors { get; set; }

        /// <summary>
        /// Gets or sets a string to be displayed to the user.
        /// </summary>
        /// <value>The user string.</value>
        public string UserString { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="T:ValidationError"/> instances.
        /// </summary>
        public List<ValidationError> ValidationErrors { get; set; }
    }
}