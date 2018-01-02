using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using GoLava.ApplePie.Transfer;

namespace GoLava.ApplePie.Clients.AppleDeveloper
{
    /// <summary>
    /// Build URLs for apple developer requests with different query parameters
    /// </summary>
    public class AppleDeveloperRequestUriBuilder
    {
        private readonly UriBuilder _uriBuilder;
        private readonly NameValueCollection _query;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:AppleDeveloperRequestUriBuilder"/> class.
        /// </summary>
        /// <param name="uri">URI.</param>
        public AppleDeveloperRequestUriBuilder(Uri uri)
        {
            _uriBuilder = new UriBuilder(uri);
            _query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            _query["content-type"] = "text/x-url-arguments";
            _query["accept"] = "application/json";
            _query["userLocale"] = "en_US";
        }

        /// <summary>
        /// Adds the query values to the URI.
        /// </summary>
        public AppleDeveloperRequestUriBuilder AddQueryValues(Dictionary<string, string> queryValues)
        {
            if (queryValues != null)
            {
                foreach (var keyValue in queryValues)
                    _query[keyValue.Key] = keyValue.Value;
            }
            return this;
        }

        /// <summary>
        /// Returns a <see cref="T:String"/> that represents the current <see cref="T:AppleDeveloperRequestUriBuilder"/> state of the URI to build.
        /// </summary>
        public override string ToString()
        {
            _query["requestId"] = Guid.NewGuid().ToString("D");
            _uriBuilder.Query = _query.ToString();

            return _uriBuilder.ToString();
        }

        /// <summary>
        /// Returns a <see cref="T:RestUri"/>  that represents the current <see cref="T:AppleDeveloperRequestUriBuilder"/> state of the URI to build.
        /// </summary>
        /// <returns>The URI.</returns>
        public RestUri ToUri()
        {
            return new RestUri(this.ToString());
        }
    }
}
