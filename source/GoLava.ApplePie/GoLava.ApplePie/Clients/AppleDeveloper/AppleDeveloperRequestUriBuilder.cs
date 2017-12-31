using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using GoLava.ApplePie.Transfer;

namespace GoLava.ApplePie.Clients.AppleDeveloper
{
    public class AppleDeveloperRequestUriBuilder
    {
        private readonly UriBuilder _uriBuilder;
        private readonly NameValueCollection _query;

        public AppleDeveloperRequestUriBuilder(Uri uri)
        {
            _uriBuilder = new UriBuilder(uri);
            _query = HttpUtility.ParseQueryString(_uriBuilder.Query);

            _query["content-type"] = "text/x-url-arguments";
            _query["accept"] = "application/json";
            _query["userLocale"] = "en_US";
        }

        public AppleDeveloperRequestUriBuilder AddQueryValues(Dictionary<string, string> queryValues)
        {
            if (queryValues != null)
            {
                foreach (var keyValue in queryValues)
                    _query[keyValue.Key] = keyValue.Value;
            }
            return this;
        }

        public override string ToString()
        {
            _query["requestId"] = Guid.NewGuid().ToString("D");
            _uriBuilder.Query = _query.ToString();

            return _uriBuilder.ToString();
        }

        public RestUri ToUri()
        {
            return new RestUri(this.ToString());
        }
    }
}
