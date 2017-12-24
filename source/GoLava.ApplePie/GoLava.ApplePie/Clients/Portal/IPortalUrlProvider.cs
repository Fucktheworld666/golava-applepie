namespace GoLava.ApplePie.Clients.Portal
{
    public interface IPortalUrlProvider : IUrlProvider
    {
        string DevicesUrl { get; }

        string AddDevicesUrl { get; }

        string TeamsUrl { get; }

        string TeamMembersUrl { get; }

        string TeamInvitesUrl { get; }
    }
}