using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Contracts.Attributes;
using GoLava.ApplePie.Contracts.Portal;
using GoLava.ApplePie.Exceptions;
using GoLava.ApplePie.Threading;
using GoLava.ApplePie.Transfer;

namespace GoLava.ApplePie.Clients.Portal
{
    public class PortalClient : ClientBase<IPortalUrlProvider>
    {
        public PortalClient(IPortalUrlProvider urlProvider)
                : base(urlProvider) { }

        public PortalClient(RestClient restClient, IPortalUrlProvider urlProvider)
            : base(restClient, urlProvider) { }

        public async Task<List<Team>> GetTeamsAsync(ClientContext context)
        {
            await Configure.AwaitFalse();

            if (!context.TryGetValue(out List<Team> teams))
            {
                var request = RestRequest.Post(new RestUri(this.UrlProvider.TeamsUrl));
                var response = await this.SendAsync<TeamsResult>(context, request);

                teams = response.Content.Teams;
                context.AddValue(teams);
            }
            return teams;
        }

        public async Task<List<Device>> GetDevicesAsync(ClientContext context, string teamId, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            if (!context.TryGetValue(out List<PageResult<Device>> devicesResult, teamId))
            {
                devicesResult = await this.GetDevicesResultsAsync(context, teamId, platform);
                context.AddValue(devicesResult, teamId);
            }
            return devicesResult.Where(r => r.Data != null).SelectMany(r => r.Data).ToList();
        }

        public async Task<List<Device>> AddDeviceAsync(ClientContext context, string teamId, string uiid, string name, DeviceClass deviceClass, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var result = await this.AddDevicesAsync(context, teamId, new NewDevices
            {
                DeviceClasses = new List<DeviceClass> { deviceClass },
                DeviceNames = new List<string> { name },
                DeviceNumbers = new List<string> { uiid },
                Register = "single"
            }, platform);
            return result.Data;
        }

        public async Task<bool> DisableDeviceAsync(ClientContext context, string teamId, Device device, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new PortalRequestUriBuilder(new RestUri(this.UrlProvider.DeleteDeviceUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId },
                { "deviceId", device.DeviceId }
            });
            var request = RestRequest.Post(uriBuilder.ToUri(), RestContentType.FormUrlEncoded, new Null(CsrfClass.Device));
            var response = await this.SendAsync<Result>(context, request);
            this.CheckResultForErrors(response.Content);
            context.DeleteValue<PageResult<Device>>(teamId);
            return true;
        }

        public async Task<Device> EnableDeviceAsync(ClientContext context, string teamId, Device device, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new PortalRequestUriBuilder(new RestUri(this.UrlProvider.EnableDeviceUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId },
                { "deviceId", string.Empty },
                { "displayId", device.DeviceId },
                { "deviceNumber", device.DeviceNumber }
            });
            var request = RestRequest.Post(uriBuilder.ToUri(), RestContentType.FormUrlEncoded, new Null(CsrfClass.Device));
            var response = await this.SendAsync<Result<Device>>(context, request);
            this.CheckResultForErrors(response.Content);
            context.DeleteValue<PageResult<Device>>(teamId);
            return response.Content.Data;
        }

        public async Task<Device> ChangeDeviceNameAsync(ClientContext context, string teamId, Device device, string newName, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new PortalRequestUriBuilder(new RestUri(this.UrlProvider.UpdateDeviceUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId },
            });
            var request = RestRequest.Post(uriBuilder.ToUri(), RestContentType.FormUrlEncoded, new UpdateDevice {
                DeviceId = device.DeviceId,
                DeviceNumber = device.DeviceNumber,
                Name = newName
            });
            var response = await this.SendAsync<Result<Device>>(context, request);
            this.CheckResultForErrors(response.Content);
            context.DeleteValue<PageResult<Device>>(teamId);
            return response.Content.Data;
        }

        private async Task<Result<List<Device>>> AddDevicesAsync(ClientContext context, string teamId, NewDevices newDevices, Platform platform)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new PortalRequestUriBuilder(new RestUri(this.UrlProvider.AddDevicesUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId }
            });
            var request = RestRequest.Post(uriBuilder.ToUri(), RestContentType.FormUrlEncoded, newDevices);
            var response = await this.SendAsync<Result<List<Device>>>(context, request);
            this.CheckResultForErrors(response.Content);
            context.DeleteValue<PageResult<Device>>(teamId);
            return response.Content;
        }

        private async Task<List<PageResult<Device>>> GetDevicesResultsAsync(ClientContext context, string teamId, Platform platform)
        {
            await Configure.AwaitFalse();

            var request = RestRequest.Post(new RestUri(this.UrlProvider.DevicesUrl, new { platform }));
            var devicesResult = await this.SendPageRequestAsync<PageResult<Device>>(
                context, request,
                new Dictionary<string, string> {
                    { "teamId", teamId },
                    { "includeRemovedDevices", "true"}
                }
            );
            return devicesResult;
        }

        private async Task<List<TPageResult>> SendPageRequestAsync<TPageResult>(
            ClientContext context, 
            RestRequest request,
            Dictionary<string, string> queryValues = null,
            string sortColumn = "name",
            int pageSize = 500)
            where TPageResult: PageResult
        {
            await Configure.AwaitFalse();

            var uriBuilder = new PortalRequestUriBuilder(request.Uri);
            uriBuilder.AddQueryValues(queryValues);

            var pages = new List<TPageResult>();

            var pageNumber = 1;
            var nd = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var total = 0;
            var sort = sortColumn + "=asc";
            var count = 0;
            do
            {
                request.Uri = uriBuilder.ToUri();
                request.Content = new
                {
                    nd,
                    pageNumber,
                    pageSize,
                    sort
                };
                request.ContentType = RestContentType.FormUrlEncoded;

                var response = await this.SendAsync<TPageResult>(context, request);
                var page = response.Content;
                this.CheckResultForErrors(page);

                var recordsCount = page.RecordsCount;
                if (recordsCount <= 0)
                    break;
                count += recordsCount;

                pages.Add(page);

                total = Math.Max(total, page.TotalRecords);

                pageNumber++;
            } 
            while (count < total);

            return pages;
        }

        private void CheckResultForErrors(Result result)
        {
            if (!string.IsNullOrEmpty(result.UserString))
                throw new ApplePieException(result.UserString);

            if (result.ValidationMessages != null && result.ValidationMessages.Count > 0)
            {
                var validationMessage = result.ValidationMessages.First();
                throw new ApplePieException(validationMessage.ValidationUserMessage);
            }
        }
    }
}