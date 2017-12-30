using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GoLava.ApplePie.Transfer.Cookies
{
    public partial class CookieJar
    {
        internal class CookieJarEnumerator : IEnumerator<DomainCookies>
        {
            private readonly IEnumerator<KeyValuePair<string, ConcurrentDictionary<string, Cookie>>> _enumerator;

            public CookieJarEnumerator(ConcurrentDictionary<string, ConcurrentDictionary<string, Cookie>> store)
            {
                _enumerator = store.GetEnumerator();
            }

            public DomainCookies Current 
            { 
                get 
                {
                    var current = _enumerator.Current;
                    return new DomainCookies
                    {
                        Domain = current.Key,
                        Cookies = current.Value.Values.ToList()
                    };
                }
            }

            object IEnumerator.Current => this.Current;

            public void Dispose()
            {
                _enumerator.Dispose();
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }
    }
}