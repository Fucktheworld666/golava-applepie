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

        public AppleDeveloperClient()
            : this(new AppleDeveloperUrlProvider()) { }

        public AppleDeveloperClient(IAppleDeveloperUrlProvider urlProvider)
                : base(urlProvider) { }

        public AppleDeveloperClient(RestClient restClient, IAppleDeveloperUrlProvider urlProvider)
            : base(restClient, urlProvider) { }

        public async Task<List<Team>> GetTeamsAsync(ClientContext context)
        {
            await Configure.AwaitFalse();

            if (context.IsForceFromBackend || !context.TryGetValue(out List<Team> teams))
            {
                var request = RestRequest.Post(new RestUri(this.UrlProvider.GetTeamsUrl));
                var response = await this.SendAsync<Result<List<Team>>>(context, request);

                teams = response.Content.Data;
                context.AddValue(teams);
            }
            return teams;
        }

        public async Task<Application> AddApplicationAsync(ClientContext context, NewApplication addApplication, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var team = await this.GetTeamAsync(context.AsCacheContext());
            return await this.AddApplicationAsync(context, team.TeamId, addApplication, platform);
        }

        public async Task<Application> AddApplicationAsync(ClientContext context, string teamId, NewApplication newApplication, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var prefixes = await this.GetApplicationPrefixesAsync(context.AsCacheContext(), teamId, platform);
            newApplication.Prefix = prefixes.First();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(
                new RestUri(this.UrlProvider.AddApplicationUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", teamId }
            });
            var request = RestRequest.Post(uriBuilder.ToUri(), RestContentType.FormUrlEncoded, newApplication);
            var response = await this.SendAsync<Result<Application>>(context, request);
            this.CheckResultForErrors(response.Content);

            context.DeleteValue<List<Application>>(teamId, platform);

            var application = response.Content.Data;
            application.TeamId = teamId;

            return application;
        }

        public async Task<bool> DeleteApplicationAsync(ClientContext context, Application application, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(
                new RestUri(this.UrlProvider.DeleteApplicationUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", application.TeamId },
                { "appIdId", application.Id }
            });
            var request = RestRequest.Post(uriBuilder.ToUri());
            var response = await this.SendAsync<Result<Application>>(context, request);
            this.CheckResultForErrors(response.Content);

            context.DeleteValue<List<Application>>(application.TeamId, platform);

            return true;
        }

        public async Task<List<Application>> GetApplicationsAsync(ClientContext context, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var team = await this.GetTeamAsync(context.AsCacheContext());
            return await this.GetApplicationsAsync(context, team.TeamId, platform);
        }

        public async Task<List<Application>> GetApplicationsAsync(ClientContext context, string teamId, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            if (context.IsForceFromBackend || !context.TryGetValue(out List<Application> applications, teamId, platform))
            {
                var applicationsResult = await this.GetApplicationsResultsAsync(context, teamId, platform);

                applications = applicationsResult.Where(r => r.Data != null).SelectMany(r => r.Data).ToList();
                applications.ForEach(a => a.TeamId = teamId);

                context.AddValue(applications, teamId, platform);
            }
            return applications;
        }

        public async Task<ApplicationDetails> GetApplicationDetails(ClientContext context, Application application, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            if (context.IsForceFromBackend 
                || !context.TryGetValue(out ApplicationDetails applicationDetails, application.TeamId, application.Id, platform))
            {
                var uriBuilder = new AppleDeveloperRequestUriBuilder(
                    new RestUri(this.UrlProvider.GetApplicationDetailsUrl, new { platform }));
                uriBuilder.AddQueryValues(new Dictionary<string, string> {
                    { "teamId", application.TeamId }
                });
                var request = RestRequest.Post(uriBuilder.ToUri(), RestContentType.FormUrlEncoded, new {
                    appIdId = application.Id
                });
                var response = await this.SendAsync<Result<ApplicationDetails>>(context, request);
                this.CheckResultForErrors(response.Content);

                applicationDetails = response.Content.Data;
                applicationDetails.TeamId = application.TeamId;

                context.AddValue(applicationDetails, application.TeamId, application.Id, platform);
            }
            return applicationDetails;
        }

        public async Task<ApplicationDetails> UpdateApplicationFeatureAsync<TFeatureValue>(
            ClientContext context, ApplicationDetails applicationDetails, 
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

            var jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();
            var featureType = jsonProperty?.PropertyName ?? _resolver.GetResolvedPropertyName(property.Name);
            var featureValue = typeof(TFeatureValue) == typeof(bool)
                ? ((bool)(object)value ? "yes" : "no")
                : value.ToString();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(
               new RestUri(this.UrlProvider.UpdateApplicationUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", applicationDetails.TeamId },
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

        public async Task<List<CertificateRequest>> GetCertificateRequestsAsync(ClientContext context, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var team = await this.GetTeamAsync(context.AsCacheContext());
            return await this.GetCertificateRequestsAsync(context, team.TeamId, platform);
        }

        public async Task<List<CertificateRequest>> GetCertificateRequestsAsync(ClientContext context, string teamId, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            if (context.IsForceFromBackend || !context.TryGetValue(out List<CertificateRequest> certificateRequests, teamId, platform))
            {
                var certificateRequestsResults = await this.GetCertificateRequestsResultsAsync(
                    context, teamId, platform);

                certificateRequests = certificateRequestsResults
                    .Where(r => r.Data != null).SelectMany(r => r.Data).ToList();
                certificateRequests.ForEach(d => d.TeamId = teamId);

                context.AddValue(certificateRequests, teamId, platform);
            }
            return certificateRequests;
        }

        public async Task<List<Device>> GetDevicesAsync(ClientContext context, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var team = await this.GetTeamAsync(context.AsCacheContext());
            return await this.GetDevicesAsync(context, team.TeamId, platform);
        }

        public async Task<List<Device>> GetDevicesAsync(ClientContext context, string teamId, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            if (context.IsForceFromBackend || !context.TryGetValue(out List<Device> devices, teamId, platform))
            {
                var devicesResult = await this.GetDevicesResultsAsync(context, teamId, platform);

                devices = devicesResult.Where(r => r.Data != null).SelectMany(r => r.Data).ToList();
                devices.ForEach(d => d.TeamId = teamId);

                context.AddValue(devices, teamId, platform);
            }
            return devices;
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

        public async Task<bool> DisableDeviceAsync(ClientContext context, Device device, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(new RestUri(this.UrlProvider.DeleteDeviceUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", device.TeamId },
                { "deviceId", device.DeviceId }
            });
            var request = RestRequest.Post(uriBuilder.ToUri());
            var response = await this.SendAsync<Result<Device>>(context, request);
            this.CheckResultForErrors(response.Content);

            context.DeleteValue<List<Device>>(device.TeamId, platform);

            return true;
        }

        public async Task<Device> EnableDeviceAsync(ClientContext context, Device device, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(new RestUri(this.UrlProvider.EnableDeviceUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", device.TeamId },
                { "deviceId", string.Empty },
                { "displayId", device.DeviceId },
                { "deviceNumber", device.DeviceNumber }
            });
            var request = RestRequest.Post(uriBuilder.ToUri());
            var response = await this.SendAsync<Result<Device>>(context, request);
            this.CheckResultForErrors(response.Content);

            context.DeleteValue<List<Device>>(device.TeamId, platform);

            return response.Content.Data;
        }

        public async Task<Device> ChangeDeviceNameAsync(ClientContext context, Device device, string newName, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            var uriBuilder = new AppleDeveloperRequestUriBuilder(new RestUri(this.UrlProvider.UpdateDeviceUrl, new { platform }));
            uriBuilder.AddQueryValues(new Dictionary<string, string> {
                { "teamId", device.TeamId },
            });
            var request = RestRequest.Post(uriBuilder.ToUri(), RestContentType.FormUrlEncoded, new UpdateDevice {
                DeviceId = device.DeviceId,
                DeviceNumber = device.DeviceNumber,
                Name = newName
            });
            var response = await this.SendAsync<Result<Device>>(context, request);
            this.CheckResultForErrors(response.Content);

            context.DeleteValue<List<Device>>(device.TeamId, platform);

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

            context.DeleteValue<List<Device>>(teamId, platform);

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

        private async Task<List<string>> GetApplicationPrefixesAsync(ClientContext context, string teamId, Platform platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            if (context.IsForceFromBackend || !context.TryGetValue(out List<string> prefixes, teamId, platform))
            {
                var request = RestRequest.Post(
                    new RestUri(this.UrlProvider.GetApplicationPrefixesUrl, new { platform }),
                    RestContentType.FormUrlEncoded, new { teamId });
                var response = await this.SendAsync<ApplicationPrefixes>(context, request);
                this.CheckResultForErrors(response.Content);

                prefixes = response.Content.AppIdPrefixes;
                context.AddValue(prefixes, teamId, platform);
            }
            return prefixes;
        }

        private async Task<List<PageResult<CertificateRequest>>> GetCertificateRequestsResultsAsync(ClientContext context, string teamId, Platform platform)
        {
            await Configure.AwaitFalse();

            var request = RestRequest.Post(new RestUri(this.UrlProvider.GetCertificateRequestsUrl, new { platform }));
            var certificateRequestsResults = await this.SendPageRequestAsync<PageResult<CertificateRequest>>(
                context, request,
                new Dictionary<string, string> {
                    { "teamId", teamId },
                    { "types", CertificateTypeDisplayIds.GetPlatformIds(platform).ToStringValue() }
                }
            );
            return certificateRequestsResults;
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

        private async Task<Team> GetTeamAsync(ClientContext context)
        {
            await Configure.AwaitFalse();

            var teams = await this.GetTeamsAsync(context);
            if (teams == null || teams.Count == 0)
            {
                var user = context.Session.User;
                throw new ApplePieException($"User '{user.FullName} ({user.Id})' does not have access to any teams with an active membership.");
            }
            if (teams.Count > 1)
            {
                // todo: log
            }

            return teams[0];
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