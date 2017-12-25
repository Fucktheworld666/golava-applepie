namespace GoLava.ApplePie.Clients.Portal
{
    public class PortalUrlProvider : UrlProviderBase, IPortalUrlProvider
    {
        public const string ProtocolVersion = "QH65B2";

        public const string BaseUrl = "https://developer.apple.com/services-account/" + ProtocolVersion + "/account/";

        public const string PlatformBaseUrl = BaseUrl + "{platform}/";

        public const string DeviceBaseUrl = PlatformBaseUrl + "device/";

        public string DevicesUrl => DeviceBaseUrl + "listDevices.action";

        public string AddDevicesUrl => DeviceBaseUrl + "addDevices.action";

        public string DeleteDeviceUrl => DeviceBaseUrl + "deleteDevice.action";

        public string EnableDeviceUrl => DeviceBaseUrl + "enableDevice.action";

        public string UpdateDeviceUrl => DeviceBaseUrl + "updateDevice.action";

        public string TeamsUrl => BaseUrl + "getTeams";

        public string TeamMembersUrl => BaseUrl + "getTeamMembers";

        public string TeamInvitesUrl => BaseUrl + "getInvites";
    }
}
