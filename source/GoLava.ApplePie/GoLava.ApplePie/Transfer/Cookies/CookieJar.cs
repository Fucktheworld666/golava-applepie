using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GoLava.ApplePie.Transfer.Cookies
{
    /// <summary>
    /// A container to store <see cref="T:Cookie"/> objects associated with <see cref="T:Uri"/>s.
    /// </summary>
    public partial class CookieJar : IEnumerable<DomainCookies>
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Cookie>> _store;
        private readonly CookieParser _cookieParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CookieJar"/> class.
        /// </summary>
        public CookieJar()
        {
            _store = new ConcurrentDictionary<string, ConcurrentDictionary<string, Cookie>>(StringComparer.OrdinalIgnoreCase);
            _cookieParser = new CookieParser();
        }

        public CookieJar(IEnumerable<DomainCookies> domainCookiesCollection)
            : this()
        {
            this.Assign(domainCookiesCollection);
        }

        /// <summary>
        /// Adds a <see cref="T:Cookie"/> instance to the <see cref="T:CookieJar"/> for a particular <see cref="T:Uri"/>.
        /// </summary>
        /// <param name="uri">The <see cref="T:Uri"/> of the <see cref="T:Cookie"/> to be added to the <see cref="T:CookieJar"/>.</param>
        /// <param name="cookie">The <see cref="T:Cookie"/> to be added to the <see cref="T:CookieJar"/>.</param>
        public void Add(Uri uri, Cookie cookie)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (cookie == null)
                throw new ArgumentNullException(nameof(cookie));

            var domain = EnsureDomain(uri, cookie);
            var cookies = _store.GetOrAdd(domain, x => this.CreateCookieDictionary());
            cookies.AddOrUpdate(cookie.Name, cookie, (k, c) => cookie);
        }

        /// <summary>
        /// Adds a one or more <see cref="T:Cookie"/> instances, represented as set-cookie header strings, 
        /// to the <see cref="T:CookieJar"/> for a particular <see cref="T:Uri"/>.
        /// </summary>
        /// <param name="uri">The <see cref="T:Uri"/> of the <see cref="T:Cookie"/> to be added to the <see cref="T:CookieJar"/>.</param>
        /// <param name="cookieHeaderValues">One or more set-cookie header to be parsed and added as <see cref="T:Cookie"/> to the <see cref="T:CookieJar"/>.</param>
        public void Add(Uri uri, IEnumerable<string> cookieHeaderValues)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (cookieHeaderValues == null)
                throw new ArgumentNullException(nameof(cookieHeaderValues));

            foreach (var cookie in _cookieParser.Parse(cookieHeaderValues))
                this.Add(uri, cookie);
        }

        /// <summary>
        /// Removes all <see cref="T:Cookie"/> instances from the <see cref="T:CookieJar"/>.
        /// </summary>
        public void Clear()
        {
            _store.Clear();
        }

        /// <summary>
        /// Gets a <see cref="T:CookieCollection"/> that contains the <see cref="T:Cookie"/> instances that are associated with a specific <see cref="T:Uri"/>.
        /// </summary>
        /// <returns>A <see cref="T:CookieCollection"/> that contains the <see cref="T:Cookie"/> instances that are associated with a specific <see cref="T:Uri"/>.</returns>
        /// <param name="uri">The <see cref="T:Uri"/> of the Cookie instances desired.</param>
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

        /// <summary>
        /// Gets the HTTP request cookie header that contains the HTTP cookies that represent 
        /// the <see cref="T:Cookie"/> instances that are associated with a specific <see cref="T:Uri"/>.
        /// </summary>
        /// <returns>A HTTP request cookie header, with strings representing <see cref="T:Cookie"/> instances delimited by semicolons.</returns>
        /// <param name="uri">URI.</param>
        public string GetRequestHeaderValue(Uri uri)
        {
            var sb = new StringBuilder();
            foreach (var cookie in this.GetCookies(uri).Cast<Cookie>()
                     .Where(c => !string.IsNullOrWhiteSpace(c.Name))
                     .OrderBy(c => c.Name))
            {
                if (sb.Length > 0)
                    sb.Append("; ");
                sb.AppendFormat("{0}={1}", cookie.Name, cookie.Value);
            }
            return sb.ToString();
        }

        private void Assign(IEnumerable<DomainCookies> domainCookiesCollection)
        {
            if (domainCookiesCollection == null)
                throw new ArgumentNullException(nameof(domainCookiesCollection));
            
            foreach (var domainCookies in domainCookiesCollection)
            {
                if (!domainCookies.Domain.StartsWith(".", StringComparison.InvariantCulture))
                    throw new ArgumentException("Domain must start with a dot.", nameof(domainCookiesCollection));

                var cookies = this.CreateCookieDictionary();
                foreach (var cookie in domainCookies.Cookies)
                    cookies.AddOrUpdate(cookie.Name, cookie, (k, c) => cookie);
                _store[domainCookies.Domain] = cookies;
            }
        }

        private ConcurrentDictionary<string, Cookie> CreateCookieDictionary()
        {
            return new ConcurrentDictionary<string, Cookie>(StringComparer.OrdinalIgnoreCase);
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

        IEnumerator<DomainCookies> IEnumerable<DomainCookies>.GetEnumerator()
        {
            return new CookieJarEnumerator(_store);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CookieJarEnumerator(_store);
        }
    }
}