namespace GoLava.ApplePie.Clients.Portal
{
    public class PortalUrlProvider : UrlProviderBase, IPortalUrlProvider
    {
        public const string ProtocolVersion = "QH65B2";

        public const string BaseUrl = "https://developer.apple.com/services-account/" + ProtocolVersion + "/account/";

        public string DevicesUrl => BaseUrl + "{platform}/device/listDevices.action";

        public string AddDevicesUrl => BaseUrl + "{platform}/device/addDevices.action";

        public string TeamsUrl => BaseUrl + "getTeams";

        public string TeamMembersUrl => BaseUrl + "getTeamMembers";

        public string TeamInvitesUrl => BaseUrl + "getInvites";
    }
}
