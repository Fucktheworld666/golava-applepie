using System.Collections.Generic;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    public class Person
    {
        public string PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string DeveloperStatus { get; set; }

        public string TeamMemberId { get; set; }

        public List<string> Roles { get; set; }
    }
}