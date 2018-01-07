using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using RichardSzalay.MockHttp;

namespace GoLava.ApplePie.Tests
{
    /// <summary>
    /// Provides extension methods for <see cref="T:MockedRequest"/>
    /// </summary>
    public static class MockedRequestExtensions
    {
        /// <summary>
        /// Sets the response of the current <see cref="T:MockedRequest"/>
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="statusCode">The <see cref="T:HttpStatusCode"/> of the response</param>
        /// <param name="headers">A list of HTTP header name/value pairs to add to the response.</param>
        /// <param name="content">The content of the response</param>
        public static MockedRequest Respond(this MockedRequest source, HttpStatusCode statusCode, IEnumerable<KeyValuePair<string, string>> headers, HttpContent content)
        {
            return source.Respond(req =>
            {
                var res = new HttpResponseMessage(statusCode)
                {
                    Content = content
                };
                foreach (var header in headers)
                {
                    res.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                return res;
            });
        }

        /// <summary>
        /// Sets the response of the current <see cref="T:MockedRequest"/>, with an OK (200) status code
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="headers">A list of HTTP header name/value pairs to add to the response.</param>
        /// <param name="content">The content of the response</param>
        public static MockedRequest Respond(this MockedRequest source, IEnumerable<KeyValuePair<string, string>> headers, HttpContent content)
        {
            return source.Respond(HttpStatusCode.OK, headers, content);
        }

        /// <summary>
        /// Sets the response of the current <see cref="T:MockedRequest"/>
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="statusCode">The <see cref="T:HttpStatusCode"/> of the response</param>
        /// <param name="headers">A list of HTTP header name/value pairs to add to the response.</param>
        /// <param name="mediaType">The media type of the response</param>
        /// <param name="content">The content of the response</param>
        public static MockedRequest Respond(this MockedRequest source, HttpStatusCode statusCode, IEnumerable<KeyValuePair<string, string>> headers, string mediaType, string content)
        {
            return source.Respond(statusCode, headers, _ => new StringContent(content, Encoding.UTF8, mediaType));
        }

        /// <summary>
        /// Sets the response of the current <see cref="T:MockedRequest"/>, with an OK (200) status code
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="headers">A list of HTTP header name/value pairs to add to the response.</param>
        /// <param name="content">The content of the response</param>
        /// <param name="mediaType">The media type of the response</param>
        public static MockedRequest Respond(this MockedRequest source, IEnumerable<KeyValuePair<string, string>> headers, string mediaType, string content)
        {
            return source.Respond(HttpStatusCode.OK, headers, mediaType, content);
        }

        /// <summary>
        /// Sets the response of the current <see cref="T:MockedRequest"/>
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="statusCode">The <see cref="T:HttpStatusCode"/> of the response</param>
        /// <param name="headers">A list of HTTP header name/value pairs to add to the response.</param>
        /// <param name="content">The content of the response</param>
        /// <param name="mediaType">The media type of the response</param>
        public static MockedRequest Respond(this MockedRequest source, HttpStatusCode statusCode, IEnumerable<KeyValuePair<string, string>> headers, string mediaType, Stream content)
        {
            return source.Respond(statusCode, headers, _ =>
            {
                if (content.CanSeek)
                {
                    content.Seek(0L, SeekOrigin.Begin);
                }

                var ms = new MemoryStream();
                content.CopyTo(ms);
                ms.Seek(0L, SeekOrigin.Begin);

                var streamContent = new StreamContent(ms);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

                return streamContent;
            });
        }

        /// <summary>
        /// Sets the response of the current <see cref="T:MockedRequest"/>
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="headers">A list of HTTP header name/value pairs to add to the response.</param>
        /// <param name="mediaType">The media type of the response</param>
        /// <param name="handler">A delegate that will return a content stream at runtime</param>
        public static MockedRequest Respond(this MockedRequest source, IEnumerable<KeyValuePair<string, string>> headers, string mediaType, Func<HttpRequestMessage, Stream> handler)
        {
            return source.Respond(HttpStatusCode.OK, headers, mediaType, handler);
        }

        /// <summary>
        /// Sets the response of the current <see cref="T:MockedRequest"/>
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="statusCode">The <see cref="T:HttpStatusCode"/> of the response</param>
        /// <param name="headers">A list of HTTP header name/value pairs to add to the response.</param>
        /// <param name="mediaType">The media type of the response</param>
        /// <param name="handler">A delegate that will return a content stream at runtime</param>
        public static MockedRequest Respond(this MockedRequest source, HttpStatusCode statusCode, IEnumerable<KeyValuePair<string, string>> headers, string mediaType, Func<HttpRequestMessage, Stream> handler)
        {
            return source.Respond(statusCode, headers, request =>
            {
                var content = handler(request);

                var streamContent = new StreamContent(content);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

                return streamContent;
            });
        }

        /// <summary>
        /// Sets the response of the current <see cref="T:MockedRequest"/>, with an OK (200) status code
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="headers">A list of HTTP header name/value pairs to add to the response.</param>
        /// <param name="content">The content of the response</param>
        /// <param name="mediaType">The media type of the response</param>
        public static MockedRequest Respond(this MockedRequest source, IEnumerable<KeyValuePair<string, string>> headers, string mediaType, Stream content)
        {
            return source.Respond(HttpStatusCode.OK, headers, mediaType, content);
        }

        /// <summary>
        /// Sets the response of the current <see cref="T:MockedRequest"/>
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="headers">A list of HTTP header name/value pairs to add to the response.</param>
        /// <param name="handler">The delegate that will return a <see cref="T:HttpContent"/> determined at runtime</param>
        public static MockedRequest Respond(this MockedRequest source, IEnumerable<KeyValuePair<string, string>> headers, Func<HttpRequestMessage, HttpContent> handler)
        {
            return source.Respond(HttpStatusCode.OK, headers, handler);
        }

        /// <summary>
        /// Sets the response of the current <see cref="T:MockedRequest"/>
        /// </summary>
        /// <param name="source">The source mocked request</param>
        /// <param name="statusCode">The <see cref="T:HttpStatusCode"/> of the response</param>
        /// <param name="headers">A list of HTTP header name/value pairs to add to the response.</param>
        /// <param name="handler">The delegate that will return a <see cref="T:HttpContent"/> determined at runtime</param>
        public static MockedRequest Respond(this MockedRequest source, HttpStatusCode statusCode, IEnumerable<KeyValuePair<string, string>> headers, Func<HttpRequestMessage, HttpContent> handler)
        {
            return source.Respond(req =>
            {
                var res = new HttpResponseMessage(statusCode)
                {
                    Content = handler(req),
                };
                foreach (var header in headers)
                {
                    res.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                return res;
            });
        }
    }
}