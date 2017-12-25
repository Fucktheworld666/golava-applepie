using System.Net.Http;

namespace GoLava.ApplePie.Transfer.Handlers
{
    public static class HttpMessageHandlerExtensions
    {
        public static T DecorateWith<T>(this HttpMessageHandler innerHandler, T outerHandler)
            where T: DelegatingHandler 
        {
            outerHandler.InnerHandler = innerHandler;
            return outerHandler;
        }
    }
}