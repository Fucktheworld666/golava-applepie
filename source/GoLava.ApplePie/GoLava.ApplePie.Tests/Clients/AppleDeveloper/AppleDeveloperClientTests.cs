using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GoLava.ApplePie.Clients;
using GoLava.ApplePie.Clients.AppleDeveloper;
using GoLava.ApplePie.Contracts.AppleDeveloper;
using GoLava.ApplePie.Transfer;
using RichardSzalay.MockHttp;
using Xunit;

namespace GoLava.ApplePie.Tests.Clients.AppleDeveloper
{
    public class AppleDeveloperClientTests : ClientBaseTests<IAppleDeveloperUrlProvider>
    {
        private const string Expected_UserString_Error = "user-string";
        private const string Expected_Team_Id = "team-id";
        private const string Expected_Team_Name = "team-name";

        public AppleDeveloperClientTests()
            : base(new AppleDeveloperUrlProvider())
        {
        }

        [Fact]
        public async Task TeamsAreLoaded()
        {
            var mockHttp = new MockHttpMessageHandler();
            this.AddValidTeamsExpectation(mockHttp);

            var context = this.CreateClientContext();
            var client = this.CreateClient(new RestClient(mockHttp), this.UrlProvider) as AppleDeveloperClient;
            var teams = await client.GetTeamsAsync(context);

            mockHttp.VerifyNoOutstandingExpectation();
        }



        protected override ClientBase<IAppleDeveloperUrlProvider> CreateClient(RestClient restClient, IAppleDeveloperUrlProvider urlProvider)
        {
            return new AppleDeveloperClient(restClient, urlProvider);
        }

        private ClientContext CreateClientContext()
        {
            return new ClientContext
            {

            };
        }

        private void AddValidTeamsExpectation(MockHttpMessageHandler mockHttp)
        {
            mockHttp
                .Expect(HttpMethod.Post, this.UrlProvider.GetTeamsUrl)
                .WithHeaders(new[] {
                    new KeyValuePair<string, string>("Accept", "application/json"),
                    new KeyValuePair<string, string>("Accept", "text/javascript"),
                    new KeyValuePair<string, string>("X-Requested-With", "XMLHttpRequest")
                })
                .Respond("application/json", this.JsonSerializer.Serialize(
                    GetValidDataResult(() => Enumerable.Range(0, 10).Select(i => new Team
                    {
                        AdminCount = i,
                        MemberCount = i,
                        ServerCount = i,
                        TeamId = Expected_Team_Id,
                        Name = Expected_Team_Name
                    }).ToList())));
        }

        private Result<TData> GetValidDataResult<TData>(Func<TData> dataProvider)
        {
            return new Result<TData>
            {
                CreationTimestamp = DateTime.UtcNow,
                Data = dataProvider()
            };
        }

        private Result<TData> GetUserStringErrorDataResult<TData>()
        {
            return new Result<TData>
            {
                CreationTimestamp = DateTime.UtcNow,
                UserString = Expected_UserString_Error
            };
        }
    }
}