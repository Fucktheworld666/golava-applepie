using System.Threading.Tasks;
using GoLava.ApplePie.Serializers;

namespace GoLava.ApplePie.Transfer
{
    public interface IRestClient
    {
        Task<RestResponse> SendAsync(RestClientContext context, RestRequest request);

        Task<RestResponse<TContent>> SendAsync<TContent>(RestClientContext context, RestRequest request);
    }
}