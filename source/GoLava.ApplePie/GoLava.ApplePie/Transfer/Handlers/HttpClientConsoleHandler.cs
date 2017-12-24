using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GoLava.ApplePie.Transfer.Handlers
{
    public class HttpClientConsoleHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.Out.WriteLine($"Request: {request}");
            try
            {
                // base.SendAsync calls the inner handler
                var response = await base.SendAsync(request, cancellationToken);
                Console.Out.WriteLine($"Response: {response}");
                return response;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to get response: {ex}");
                throw;
            }
        }
    }
}