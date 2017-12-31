using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts.AppleDeveloper;
using GoLava.ApplePie.Exceptions;
using GoLava.ApplePie.Threading;
using GoLava.ApplePie.Transfer;
using GoLava.ApplePie.Transfer.Resolvers;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Clients.AppleDeveloper
{
    public class AppleDeveloperClient : ClientBase<IAppleDeveloperUrlProvider>
    {
        private readonly CustomPropertyNamesContractResolver _resolver = new CustomPropertyNamesContractResolver();

        public AppleDeveloperClient(IAppleDeveloperUrlProvider urlProvider)
                : base(urlProvider) { }

        public AppleDeveloperClient(RestClient restClient, IAppleDeveloperUrlProvider urlProvider)
            : base(restClient, urlProvider) { }

        public async Task<List<Team>> GetTeamsAsync(ClientContext context)
        {
            await Configure.AwaitFalse();

            if (!context.TryGetValue(out List<Team> teams))
            {
                var request = RestRequest.Post(new RestUri(this.UrlProvider.GetTeamsUrl));
                var response = await this.SendAsync<Result<List<Team>>>(context, request);

                teams = response.Content.Data;
                context.AddValue(teams);
            }
            return teams;
        }

        public async Task<Application> AddApplicationAsync(ClientContext context, string teamId, AddApplication addApplication, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(
                new RestUri(this.UrlProvider.AddApplicationUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId }
            });
            var request = RestRequest.Post(uriBuilder.ToUri(), RestContentType.FormUrlEncoded, addApplication);
            var response = await this.SendAsync<Result<Application>>(context, request);
            this.CheckResultForErrors(response.Content);
            return response.Content.Data;
        }

        public async Task<bool> DeleteApplicationAsync(ClientContext context, string teamId, Application application, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(
                new RestUri(this.UrlProvider.DeleteApplicationUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId },
                { "appIdId", application.Id }
            });
            var request = RestRequest.Post(uriBuilder.ToUri());
            var response = await this.SendAsync<Result<Application>>(context, request);
            this.CheckResultForErrors(response.Content);
            return true;
        }

        public async Task<List<Application>> GetApplicationsAsync(ClientContext context, string teamId, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            if (!context.TryGetValue(out List<PageResult<Application>> applicationsResult, teamId))
            {
                applicationsResult = await this.GetApplicationsResultsAsync(context, teamId, platform);
                context.AddValue(applicationsResult, teamId);
            }
            return applicationsResult.Where(r => r.Data != null).SelectMany(r => r.Data).ToList();
        }

        public async Task<ApplicationDetails> GetApplicationDetails(ClientContext context, string teamId, Application application, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            if (!context.TryGetValue(out ApplicationDetails applicationDetails, teamId, application.Id))
            {
                var uriBuilder = new AppleDeveloperRequestUriBuilder(
                    new RestUri(this.UrlProvider.GetApplicationDetailsUrl, new { platform }));
                uriBuilder.AddQueryValues(new Dictionary<string, string> {
                    { "teamId", teamId }
                });
                var request = RestRequest.Post(uriBuilder.ToUri(), RestContentType.FormUrlEncoded, new {
                    appIdId = application.Id
                });
                var response = await this.SendAsync<Result<ApplicationDetails>>(context, request);
                this.CheckResultForErrors(response.Content);
                applicationDetails = response.Content.Data;
                context.AddValue(applicationDetails, teamId, application.Id);
            }
            return applicationDetails;
        }

        public async Task<ApplicationDetails> UpdateApplicationFeatureAsync<TFeatureValue>(
            ClientContext context, string teamId, ApplicationDetails applicationDetails, 
            Expression<Func<ApplicationFeatures, TFeatureValue>> feature, TFeatureValue value, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var member = feature.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException($"Expression '{feature}' does not refer to a property.", nameof(feature));

            var property = member.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException($"Expression '{feature}' does not refer to a property.", nameof(feature));

            var originalValue = property.GetValue(applicationDetails.Features);
            if (value.Equals(originalValue))
                return applicationDetails;

            var valueType = typeof(TFeatureValue);
            var jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();
            var featureType = jsonProperty?.PropertyName ?? _resolver.GetResolvedPropertyName(property.Name);
            var featureValue = valueType == typeof(bool)
                ? ((bool)(object)value ? "yes" : "no")
                : value.ToString();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(
               new RestUri(this.UrlProvider.UpdateApplicationUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId },
                { "displayId", applicationDetails.Id },
                { "featureType", featureType },
                { "featureValue", featureValue }
            });
            var request = RestRequest.Post(uriBuilder.ToUri());
            var response = await this.SendAsync<Result<ApplicationDetails>>(context, request);
            this.CheckResultForErrors(response.Content);

            property.SetValue(applicationDetails.Features, value);

            return applicationDetails;
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

            var uriBuilder = new AppleDeveloperRequestUriBuilder(new RestUri(this.UrlProvider.DeleteDeviceUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId },
                { "deviceId", device.DeviceId }
            });
            var request = RestRequest.Post(uriBuilder.ToUri());
            var response = await this.SendAsync<Result<Device>>(context, request);
            this.CheckResultForErrors(response.Content);
            context.DeleteValue<PageResult<Device>>(teamId);
            return true;
        }

        public async Task<Device> EnableDeviceAsync(ClientContext context, string teamId, Device device, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(new RestUri(this.UrlProvider.EnableDeviceUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId },
                { "deviceId", string.Empty },
                { "displayId", device.DeviceId },
                { "deviceNumber", device.DeviceNumber }
            });
            var request = RestRequest.Post(uriBuilder.ToUri());
            var response = await this.SendAsync<Result<Device>>(context, request);
            this.CheckResultForErrors(response.Content);
            context.DeleteValue<PageResult<Device>>(teamId);
            return response.Content.Data;
        }

        public async Task<Device> ChangeDeviceNameAsync(ClientContext context, string teamId, Device device, string newName, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(new RestUri(this.UrlProvider.UpdateDeviceUrl, new { platform }));
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

            var uriBuilder = new AppleDeveloperRequestUriBuilder(new RestUri(this.UrlProvider.AddDevicesUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId }
            });
            var request = RestRequest.Post(uriBuilder.ToUri(), RestContentType.FormUrlEncoded, newDevices);
            var response = await this.SendAsync<Result<List<Device>>>(context, request);
            this.CheckResultForErrors(response.Content);
            context.DeleteValue<PageResult<Device>>(teamId);
            return response.Content;
        }

        private async Task<List<PageResult<Application>>> GetApplicationsResultsAsync(ClientContext context, string teamId, Platform platform)
        {
            await Configure.AwaitFalse();

            var request = RestRequest.Post(new RestUri(this.UrlProvider.GetApplicationsUrl, new { platform }));
            var applicationsResult = await this.SendPageRequestAsync<PageResult<Application>>(
                context, request,
                new Dictionary<string, string> {
                    { "teamId", teamId }
                }
            );
            return applicationsResult;
        }

        private async Task<List<PageResult<Device>>> GetDevicesResultsAsync(ClientContext context, string teamId, Platform platform)
        {
            await Configure.AwaitFalse();

            var request = RestRequest.Post(new RestUri(this.UrlProvider.GetDevicesUrl, new { platform }));
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

            var uriBuilder = new AppleDeveloperRequestUriBuilder(request.Uri);
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