using System;
using GoLava.ApplePie.Formatting;

namespace GoLava.ApplePie.Transfer
{
    public class RestUri : Uri
    {
        private static readonly NamedFormatter _namedFormatter;

        static RestUri()
        {
            _namedFormatter = new NamedFormatter();
        }


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
