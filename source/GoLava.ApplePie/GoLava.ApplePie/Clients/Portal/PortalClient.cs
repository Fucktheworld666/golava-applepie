using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GoLava.ApplePie.Contracts.Portal;
using GoLava.ApplePie.Threading;
using GoLava.ApplePie.Transfer;

namespace GoLava.ApplePie.Clients.Portal
{
    public class PortalClient : ClientBase<IPortalUrlProvider>
    {
        public PortalClient(IPortalUrlProvider urlProvider)
                : base(urlProvider) { }

        internal PortalClient(RestClient restClient, IPortalUrlProvider urlProvider)
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

        public async Task<List<Device>> GetDevicesAsync(ClientContext context, string teamId, string platform = Platform.Ios)
        {
            await Configure.AwaitFalse();

            if (!context.TryGetValue(out List<PageResult<Device>> devicesResult, teamId))
            {
                devicesResult = await this.GetDevicesResultsAsync(context, teamId, platform);
                context.AddValue(devicesResult, teamId);
            }
            return devicesResult.Where(r => r.Data != null).SelectMany(r => r.Data).ToList();
        }

        private async Task<List<PageResult<Device>>> GetDevicesResultsAsync(ClientContext context, string teamId, string platform)
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

            var uriBuilder = new UriBuilder(request.Uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["content-type"] = "text/x-url-arguments";
            query["accept"] = "application/json";
            query["userLocale"] = "en_US";

            if (queryValues != null)
            {
                foreach (var keyValue in queryValues)
                    query[keyValue.Key] = keyValue.Value;
            }

            var pages = new List<TPageResult>();

            var pageNumber = 1;
            var nd = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var total = 0;
            var sort = sortColumn + "=asc";
            var count = 0;
            do
            {
                query["requestId"] = Guid.NewGuid().ToString("D");
                uriBuilder.Query = query.ToString();

                request.Uri = new RestUri(uriBuilder.ToString());
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

                pages.Add(page);

                total = Math.Max(total, page.TotalRecords);
                count += page.RecordsCount;
                pageNumber++;
            } 
            while (count < total);

            return pages;
        }
    }
}