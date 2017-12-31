namespace GoLava.ApplePie.Clients.AppleDeveloper
{
    public class AppleDeveloperUrlProvider : UrlProviderBase, IAppleDeveloperUrlProvider
    {
        public const string ProtocolVersion = "QH65B2";

        public const string BaseUrl = "https://developer.apple.com/services-account/" + ProtocolVersion + "/account/";

        public const string PlatformBaseUrl = BaseUrl + "{platform}/";

        public const string DeviceBaseUrl = PlatformBaseUrl + "device/";

        public const string IdentifiersBaseUrl = PlatformBaseUrl + "identifiers/";

        public string AddApplicationUrl => IdentifiersBaseUrl + "addAppId.action";

        public string GetApplicationsUrl => IdentifiersBaseUrl + "listAppIds.action";

        public string GetApplicationDetailsUrl => IdentifiersBaseUrl + "getAppIdDetail.action";

        public string UpdateApplicationUrl => IdentifiersBaseUrl + "updateService.action";

        public string GetDevicesUrl => DeviceBaseUrl + "listDevices.action";

        public string AddDevicesUrl => DeviceBaseUrl + "addDevices.action";

        public string DeleteDeviceUrl => DeviceBaseUrl + "deleteDevice.action";

        public string EnableDeviceUrl => DeviceBaseUrl + "enableDevice.action";

        public string UpdateDeviceUrl => DeviceBaseUrl + "updateDevice.action";

        public string GetTeamsUrl => BaseUrl + "getTeams";

        public string GetTeamMembersUrl => BaseUrl + "getTeamMembers";

        public string GetTeamInvitesUrl => BaseUrl + "getInvites";
    }
}
