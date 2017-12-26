using System;
using GoLava.ApplePie.Formatting;

namespace GoLava.ApplePie.Transfer
{
    /// <summary>
    /// Extends the <see cref="T:Uri"/> class to allow instance replacement of named parameters.
    /// </summary>
    /// <example>
    /// var restUri = new RestUri("http://domain.ext/{foo}/index.{ext}", new { foo = "bar", ext = "html" });
    /// </example>
    public class RestUri : Uri
    {
        private static readonly NamedFormatter _namedFormatter;

        static RestUri()
        {
            _namedFormatter = new NamedFormatter();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RestUri"/> class.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="urlArguments">URL arguments.</param>
        public RestUri(string url, object urlArguments = null)
            : base(ReplaceUrlArguments(url, urlArguments))
        {
        }

        private static string ReplaceUrlArguments(string url, object urlArguments = null)
        {
            if (urlArguments != null)
                url = _namedFormatter.Format(url, urlArguments);
            return url;
        }
    }
}
