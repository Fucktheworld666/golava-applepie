using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace GoLava.ApplePie.Transfer.Cookies
{
    public class CookieParser
    {
        public IEnumerable<Cookie> Parse(IEnumerable<string> cookieHeaderValues)
        {
            if (cookieHeaderValues == null)
                throw new ArgumentNullException(nameof(cookieHeaderValues));

            foreach (var c in cookieHeaderValues)
            {
                var segments = c.Split(';');
                var nameAndValue = GetKeyValue(segments[0]);

                var cookie = new Cookie(nameAndValue.Key, nameAndValue.Value)
                {
                    // Path defaults to /, want to be able to roundtrip non-existing field.
                    Path = null
                };

                // First key-value-pair is cookie name and value, now look at the rest.
                for (int i = 1; i < segments.Length; i++)
                {
                    var kv = GetKeyValue(segments[i]);
                    switch (kv.Key.ToLowerInvariant())
                    {
                        case "expires":
                            cookie.Expires = DateTime.Parse(kv.Value, CultureInfo.InvariantCulture);
                            break;
                        case "secure":
                            cookie.Secure = true;
                            break;
                        case "httponly":
                            cookie.HttpOnly = true;
                            break;
                        case "path":
                            cookie.Path = kv.Value;
                            break;
                        case "domain":
                            cookie.Domain = kv.Value;
                            break;
                    }
                }

                yield return cookie;
            }
        }

        public static KeyValuePair<string, string> GetKeyValue(string segment)
        {
            var idx = segment.IndexOf('=');
            return idx < 0 
                ? new KeyValuePair<string, string>(segment.Trim(), null)
                : new KeyValuePair<string, string>(segment.Substring(0, idx).Trim(), segment.Substring(idx + 1, segment.Length - idx - 1));
        }
    }
}
