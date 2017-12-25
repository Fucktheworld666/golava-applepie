using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts;
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
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public RestClient()
        {
            var pipeline = new HttpClientConsoleHandler
            {
                InnerHandler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    UseCookies = false
                }
            };

            _httpClient = new HttpClient(pipeline);
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CustomPropertyNamesContractResolver()
            };
        }

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
                    var content = JsonConvert.DeserializeObject<TContent>(rawContent, _jsonSerializerSettings);
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

            var content = restRequest.Content;
            if (content is Null)
                content = null;

            HttpContent httpContent;
            switch (restRequest.ContentType)
            {
                case RestContentType.Json:
                    var json = JsonConvert.SerializeObject(content, _jsonSerializerSettings);
                    httpContent = new StringContent(json, restRequest.ContentEncoding, "application/json");
                    if (restRequest.ContentEncoding == null)
                        httpContent.Headers.ContentType.CharSet = null;
                    httpContent.Headers.ContentLength = json.Length;
                    break;
                case RestContentType.FormUrlEncoded:
                    httpContent = new CustomFormUrlEncodedContent(content);
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