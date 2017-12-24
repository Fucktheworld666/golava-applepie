using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GoLava.ApplePie.Transfer.Cookies
{
    public class CookieJar
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Cookie>> _store;
        private readonly CookieParser _cookieParser;

        public CookieJar()
        {
            _store = new ConcurrentDictionary<string, ConcurrentDictionary<string, Cookie>>(StringComparer.OrdinalIgnoreCase);
            _cookieParser = new CookieParser();
        }

        public void Add(Uri uri, Cookie cookie)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (cookie == null)
                throw new ArgumentNullException(nameof(cookie));

            var domain = EnsureDomain(uri, cookie);
            var cookies = _store.GetOrAdd(domain, x => new ConcurrentDictionary<string, Cookie>(StringComparer.OrdinalIgnoreCase));
            cookies.AddOrUpdate(cookie.Name, cookie, (k, c) => cookie);
        }

        public void Add(Uri uri, IEnumerable<string> cookieHeaderValues)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (cookieHeaderValues == null)
                throw new ArgumentNullException(nameof(cookieHeaderValues));

            foreach (var cookie in _cookieParser.Parse(cookieHeaderValues))
                this.Add(uri, cookie);
        }

        public void Clear()
        {
            _store.Clear();
        }

        public CookieCollection GetCookies(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var collection = new CookieCollection();
            foreach (var subDomain in GetDomainParts(uri))
            {
                if (_store.TryGetValue(subDomain, out ConcurrentDictionary<string, Cookie> cookies))
                {
                    foreach (var cookie in cookies.Values)
                        collection.Add(cookie);
                }
            }
            return collection;
        }

        public string GetRequestHeaderValue(Uri uri)
        {
            var sb = new StringBuilder();
            foreach (var cookie in this.GetCookies(uri).Cast<Cookie>().OrderBy(c => c.Name))
            {
                if (sb.Length > 0)
                    sb.Append("; ");
                sb.AppendFormat("{0}={1}", cookie.Name, cookie.Value);
            }
            return sb.ToString();
        }

        private static string EnsureDomain(Uri uri, Cookie cookie)
        {
            var domain = cookie.Domain;
            if (string.IsNullOrEmpty(domain))
                return "." + uri.Host;
            if (!domain.StartsWith(".", StringComparison.Ordinal))
                domain = "." + domain;

            foreach (var subDomain in GetDomainParts(uri))
            {
                if (domain == subDomain)
                    return domain;
            }

            throw new CookieException();
        }

        private static IEnumerable<string> GetDomainParts(Uri uri)
        {
            var parts = uri.Host.Split('.');
            for (var i = 0; i < parts.Length - 1; i++)
            {
                var subDomain = string.Join(".", parts.Skip(i));
                yield return "." + subDomain;
            }
        }
    }
}