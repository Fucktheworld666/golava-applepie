using System;
using GoLava.ApplePie.Contracts.Converters;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    /// <summary>
    /// Provides information about a team of a user.
    /// </summary>
    public class Team
    {
        /// <summary>
        /// Gets or sets the team identifier.
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the team.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the address of the team.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the agent of the team.
        /// </summary>
        public Person Agent { get; set; }

        /// <summary>
        /// Gets or sets the program of the team.
        /// </summary>
        public Program Program { get; set; }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        public string UserRole { get; set; }

        /// <summary>
        /// Gets or sets the team member identifier.
        /// </summary>
        public string TeamMemberId { get; set; }

        /// <summary>
        /// Gets or sets the count of admins of the team.
        /// </summary>
        public int AdminCount { get; set; }

        /// <summary>
        /// Gets or sets the count of members of the team.
        /// </summary>
        public int MemberCount { get; set; }

        /// <summary>
        /// Gets or sets the count of servers of the team.
        /// </summary>
        public int ServerCount { get; set; }

        /// <summary>
        /// Gets or sets the next device reset date.
        /// </summary>
        [JsonConverter(typeof(MillisecondsEpochConverter))]
        public DateTime NextDeviceResetDate { get; set; }
    }
}