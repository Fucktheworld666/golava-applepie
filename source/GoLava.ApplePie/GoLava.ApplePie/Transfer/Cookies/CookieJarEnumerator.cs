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
            private readonly IEnumerator<KeyValuePair<string, ConcurrentDictionary<string, Cookie>>> _secureEnumerator;
            private IEnumerator<KeyValuePair<string, ConcurrentDictionary<string, Cookie>>> _currentEnumerator;
                

            public CookieJarEnumerator(
                ConcurrentDictionary<string, ConcurrentDictionary<string, Cookie>> store,
                ConcurrentDictionary<string, ConcurrentDictionary<string, Cookie>> secureStore)
            {
                _enumerator = store.GetEnumerator();
                _secureEnumerator = secureStore.GetEnumerator();
            }

            public DomainCookies Current 
            { 
                get 
                {
                    var current = _currentEnumerator.Current;
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
                _secureEnumerator.Dispose();
                _currentEnumerator = null;
            }

            public bool MoveNext()
            {
                if (_currentEnumerator == null)
                    _currentEnumerator = _enumerator;

                var result = _currentEnumerator.MoveNext();
                if (!result && _currentEnumerator != _secureEnumerator)
                {
                    _currentEnumerator = _secureEnumerator;
                    result = _currentEnumerator.MoveNext();
                }

                return result;
            }

            public void Reset()
            {
                _enumerator.Reset();
                _secureEnumerator.Reset();
                _currentEnumerator = null;
            }
        }
    }
}