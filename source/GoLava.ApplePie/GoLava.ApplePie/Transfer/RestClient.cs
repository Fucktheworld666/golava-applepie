using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Extensions;
using GoLava.ApplePie.Serializers;
using GoLava.ApplePie.Threading;
using GoLava.ApplePie.Transfer.Content;
using GoLava.ApplePie.Transfer.Handlers;
using GoLava.ApplePie.Transfer.Resolvers;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Transfer
{
    public class RestClient 
    {
        private readonly HttpClient _httpClient;

        private static HttpMessageHandler CreateHttpMessageHandlerPipeline()
        {
            var pipeline = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = false
            }.DecorateWith(new HttpClientConsoleHandler());
            return pipeline;
        }

        public RestClient()
            : this(CreateHttpMessageHandlerPipeline()) { }

        public RestClient(HttpMessageHandler httpMessageHandler)
        {
            if (httpMessageHandler == null)
                throw new ArgumentNullException(nameof(httpMessageHandler));
            
            _httpClient = new HttpClient(httpMessageHandler);

            this.Serializer = new Serializers.JsonSerializer(new JsonSerializerSettings {
                ContractResolver = new CustomPropertyNamesContractResolver()
            });
        }

        public Serializers.JsonSerializer Serializer { get; }

        public async Task<RestResponse<TContent>> SendAsync<TContent>(RestClientContext context, RestRequest request)
        {
            await Configure.AwaitFalse();

            var httpRequest = this.CreateHttpRequestMessage(context, request);
            var httpResponse = await _httpClient.SendAsync(httpRequest);

            var restResponse = await this.CreateRestResponseAsync<TContent>(context, httpRequest, httpResponse);

            return restResponse;
        }

        private HttpRequestMessage CreateHttpRequestMessage(RestClientContext context, RestRequest restRequest)
        {
            var httpRequest = new HttpRequestMessage
            {
                RequestUri = restRequest.Uri,
                Method = restRequest.Method,
                Content = this.CreateHttpContent(restRequest),
            };
            httpRequest.Headers.Host = httpRequest.RequestUri.Host;
            httpRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("GoLava", "1.0"));
            httpRequest.Headers.Add("Connection", "keep-alive");

            var cookies = context.CookieJar.GetRequestHeaderValue(httpRequest.RequestUri);
            if (!string.IsNullOrEmpty(cookies))
                httpRequest.Headers.Add("Cookie", cookies);

            if (restRequest.Headers != null)
            {
                foreach (var header in restRequest.Headers)
                    httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return httpRequest;
        }

        private async Task<RestResponse<TContent>> CreateRestResponseAsync<TContent>(RestClientContext context, HttpRequestMessage httpRequest, HttpResponseMessage httpResponse)
        {
            await Configure.AwaitFalse();

            var rawContent = await httpResponse.Content.ReadAsStringAsync();

            var restResponse = new RestResponse<TContent>
            {
                StatusCode = httpResponse.StatusCode,
                RawContent = rawContent,
                Headers = new RestHeaders(httpResponse.Headers),
                ContentType = ConvertContentType(httpResponse.Content.Headers)
            };
            if (httpResponse.IsSuccessStatusCode)
            {
                if (restResponse.ContentType == RestContentType.Json)
                {
                    var content = this.Serializer.Deserialize<TContent>(rawContent);
                    restResponse.Content = content;
                }

                if (httpResponse.Headers.TryGetValues("set-cookie", out IEnumerable<string> cookieHeaders))
                    context.CookieJar.Add(httpRequest.RequestUri, cookieHeaders);
            }
            return restResponse;
        }

        private RestContentType ConvertContentType(HttpContentHeaders contentHeaders)
        {
            switch (contentHeaders.ContentType.MediaType.ToLowerInvariant())
            {
                case "text/plain":
                    return RestContentType.Text;
                case "text/html":
                    return RestContentType.Html;
                case "application/json":
                case "text/javascript":
                    return RestContentType.Json;
            }

            return RestContentType.None;
        }

        private HttpContent CreateHttpContent(RestRequest restRequest)
        {
            if (restRequest.Method != HttpMethod.Post && restRequest.Method != HttpMethod.Put)
                return null;

            HttpContent httpContent;
            switch (restRequest.ContentType)
            {
                case RestContentType.Json:
                    var json = this.Serializer.Serialize(restRequest.Content);
                    httpContent = new StringContent(json, restRequest.ContentEncoding, "application/json");
                    if (restRequest.ContentEncoding == null)
                        httpContent.Headers.ContentType.CharSet = null;
                    httpContent.Headers.ContentLength = json.Length;
                    break;
                case RestContentType.FormUrlEncoded:
                    httpContent = new CustomFormUrlEncodedContent(restRequest.Content);
                    break;
                default:
                    httpContent = new NullContent();
                    break;
            }
            if (httpContent.Headers.ContentLength == null)
                httpContent.Headers.ContentLength = 0;
            return httpContent;
        }
    }
}