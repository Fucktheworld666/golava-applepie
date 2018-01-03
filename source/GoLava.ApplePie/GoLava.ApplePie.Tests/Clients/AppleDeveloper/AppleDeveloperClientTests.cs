using GoLava.ApplePie.Clients;
using GoLava.ApplePie.Clients.AppleDeveloper;
using GoLava.ApplePie.Transfer;

namespace GoLava.ApplePie.Tests.Clients.AppleDeveloper
{
    public class AppleDeveloperClientTests : ClientBaseTests<IAppleDeveloperUrlProvider>
    {
        public AppleDeveloperClientTests()
            : base(new AppleDeveloperUrlProvider())
        {
        }

        protected override ClientBase<IAppleDeveloperUrlProvider> CreateClient(RestClient restClient, IAppleDeveloperUrlProvider urlProvider)
        {
            return new AppleDeveloperClient(restClient, urlProvider);
        }
    }
}