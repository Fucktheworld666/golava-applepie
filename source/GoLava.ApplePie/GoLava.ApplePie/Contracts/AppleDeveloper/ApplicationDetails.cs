using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    /// <summary>
    /// The application details contract.
    /// </summary>
    public class ApplicationDetails : Application
    {
        /// <summary>
        /// Gets or sets the application features.
        /// </summary>
        public ApplicationFeatures Features { get; set; }

        /// <summary>
        /// Gets or sets the list of enabled features.
        /// </summary>
        public List<string> EnabledFeatures { get; set; }

        /// <summary>
        /// Gets or sets a value whether development push is enabled.
        /// </summary>
        public bool IsDevPushEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value whether production push is enabled.
        /// </summary>
        public bool IsProdPushEnabled { get; set; }

        /// <summary>
        /// Gets or sets the associated application groups count.
        /// </summary>
        public int AssociatedApplicationGroupsCount { get; set; }

        /// <summary>
        /// Gets or sets the associated cloud containers count.
        /// </summary>
        public int AssociatedCloudContainersCount { get; set; }

        /// <summary>
        /// Gets or sets the associated identifiers count.
        /// </summary>
        public int AssociatedIdentifiersCount { get; set; }

        /// <summary>
        /// Gets or sets the associated application groups.
        /// </summary>
        public object AssociatedApplicationGroups { get; set; }

        /// <summary>
        /// Gets or sets the associated cloud containers.
        /// </summary>
        public object AssociatedCloudContainers { get; set; }

        /// <summary>
        /// Gets or sets the associated identifiers.
        /// </summary>
        public object AssociatedIdentifiers { get; set; }
    }
}