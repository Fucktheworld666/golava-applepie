namespace GoLava.ApplePie.Clients.Portal
{
    public interface IPortalUrlProvider : IUrlProvider
    {
        string DevicesUrl { get; }

        string AddDevicesUrl { get; }

        string DeleteDeviceUrl { get; }

        string EnableDeviceUrl { get; }

        string UpdateDeviceUrl { get; }

        string TeamsUrl { get; }

        string TeamMembersUrl { get; }

        string TeamInvitesUrl { get; }
    }
}