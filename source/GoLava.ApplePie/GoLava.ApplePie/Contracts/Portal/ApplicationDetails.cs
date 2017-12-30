using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts.Portal
{
    public class ApplicationDetails : Application
    {
        public ApplicationFeatures Features { get; set; }

        public List<string> EnabledFeatures { get; set; }

        public bool IsDevPushEnabled { get; set; }

        public bool IsProdPushEnabled { get; set; }

        public int AssociatedApplicationGroupsCount { get; set; }

        public int AssociatedCloudContainersCount { get; set; }

        public int AssociatedIdentifiersCount { get; set; }

        public object AssociatedApplicationGroups { get; set; }

        public object AssociatedCloudContainers { get; set; }

        public object AssociatedIdentifiers { get; set; }
    }
}