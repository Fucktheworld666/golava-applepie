namespace GoLava.ApplePie.Clients.AppleDeveloper
{
    public interface IAppleDeveloperUrlProvider : IUrlProvider
    {
        string GetApplicationsUrl { get; }

        string GetApplicationDetailsUrl { get; }

        string UpdateApplicationUrl { get; }

        string GetDevicesUrl { get; }

        string AddDevicesUrl { get; }

        string DeleteDeviceUrl { get; }

        string EnableDeviceUrl { get; }

        string UpdateDeviceUrl { get; }

        string GetTeamsUrl { get; }

        string GetTeamMembersUrl { get; }

        string GetTeamInvitesUrl { get; }
    }
}