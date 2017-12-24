using System.Net.Http;
using System.Text;

namespace GoLava.ApplePie.Transfer
{
    public class RestRequest
    {
        public static RestRequest Get(RestUri uri) => Get(uri, null);

        public static RestRequest Get(RestUri uri, RestHeaders headers) => new RestRequest
        {
            Method = HttpMethod.Get,
            Uri = uri,
            Headers = headers
        };

        public static RestRequest Post(RestUri uri) => Post(uri, null);

        public static RestRequest Post(RestUri uri, RestHeaders headers) => Post(uri, headers, RestContentType.None, null);

        public static RestRequest Post(RestUri uri, RestContentType contentType, object content) => Post(uri, null, contentType, content);

        public static RestRequest Post(RestUri uri, RestHeaders headers, RestContentType contentType, object content) => new RestRequest
        {
            Method = HttpMethod.Post,
            Uri = uri,
            Headers = headers,
            ContentType = contentType,
            Content = content,
            ContentEncoding = Encoding.UTF8
        };

        public HttpMethod Method { get; set; }

        public RestHeaders Headers { get; set; }

        public RestUri Uri { get; set; }

        public object Content { get; set; }

        public RestContentType ContentType { get; set; }

        public Encoding ContentEncoding { get; set; }
    }
}
