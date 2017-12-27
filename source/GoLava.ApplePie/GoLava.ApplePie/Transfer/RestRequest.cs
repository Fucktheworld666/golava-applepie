using System.Net.Http;
using System.Text;

namespace GoLava.ApplePie.Transfer
{
    /// <summary>
    /// Request send via a <see cref="T:RestClient"/>. 
    /// </summary>
    public class RestRequest
    {
        /// <summary>
        /// Creates a GET rest request.
        /// </summary>
        public static RestRequest Get(RestUri uri) => Get(uri, null);

        /// <summary>
        /// Creates a GET rest request.
        /// </summary>
        public static RestRequest Get(RestUri uri, RestHeaders headers) => new RestRequest
        {
            Method = HttpMethod.Get,
            Uri = uri,
            Headers = headers
        };

        /// <summary>
        /// Creates a POST rest request.
        /// </summary>
        public static RestRequest Post(RestUri uri) => Post(uri, null);

        /// <summary>
        /// Creates a POST rest request.
        /// </summary>
        public static RestRequest Post(RestUri uri, RestHeaders headers) => Post(uri, headers, RestContentType.None, null);

        /// <summary>
        /// Creates a POST rest request.
        /// </summary>
        public static RestRequest Post(RestUri uri, RestContentType contentType, object content) => Post(uri, null, contentType, content);

        /// <summary>
        /// Creates a POST rest request.
        /// </summary>
        public static RestRequest Post(RestUri uri, RestHeaders headers, RestContentType contentType, object content) => new RestRequest
        {
            Method = HttpMethod.Post,
            Uri = uri,
            Headers = headers,
            ContentType = contentType,
            Content = content,
            ContentEncoding = Encoding.UTF8
        };

        /// <summary>
        /// Creates a PUT rest request.
        /// </summary>
        public static RestRequest Put(RestUri uri) => Put(uri, null);

        /// <summary>
        /// Creates a PUT rest request.
        /// </summary>
        public static RestRequest Put(RestUri uri, RestHeaders headers) => Put(uri, headers, RestContentType.None, null);

        /// <summary>
        /// Creates a PUT rest request.
        /// </summary>
        public static RestRequest Put(RestUri uri, RestContentType contentType, object content) => Put(uri, null, contentType, content);

        /// <summary>
        /// Creates a PUT rest request.
        /// </summary>
        public static RestRequest Put(RestUri uri, RestHeaders headers, RestContentType contentType, object content) => new RestRequest
        {
            Method = HttpMethod.Put,
            Uri = uri,
            Headers = headers,
            ContentType = contentType,
            Content = content,
            ContentEncoding = Encoding.UTF8
        };


        /// <summary>
        /// Gets or sets the <see cref="T:HttpMethod"/> of the rest request.
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:RestHeaders"/> of the rest request.
        /// </summary>
        public RestHeaders Headers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:RestUri"/> of the rest request.
        /// </summary>
        public RestUri Uri { get; set; }

        /// <summary>
        /// Gets or sets the content of the rest request.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="RestContentType"/> of the rest request.
        /// </summary>
        public RestContentType ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content <see cref="T:Encoding"/> of the rest request.
        /// </summary>
        public Encoding ContentEncoding { get; set; }
    }
}