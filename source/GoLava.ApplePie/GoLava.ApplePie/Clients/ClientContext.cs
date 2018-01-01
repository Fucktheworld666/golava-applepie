using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Contracts.Attributes;
using GoLava.ApplePie.Transfer;

namespace GoLava.ApplePie.Clients
{
    /// <summary>
    /// Context that keeps data between calls.
    /// </summary>
    public class ClientContext : RestClientContext
    {
        private readonly ConcurrentDictionary<string, object> _stash;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ClientContext"/> class.
        /// </summary>
        public ClientContext()
        {
            _stash = new ConcurrentDictionary<string, object>();
        }

        private ClientContext(ConcurrentDictionary<string, object> stash)
        {
            _stash = stash;
        }

        /// <summary>
        /// Gets or sets the value of the current authentication state.
        /// </summary>
        public Authentication Authentication { get; set; }

        /// <summary>
        /// Gets or sets the auth token to be used during authenication calls.
        /// </summary>
        public AuthToken AuthToken { get; set; }

        /// <summary>
        /// Gets or sets the logon authentication information.
        /// </summary>
        public LogonAuth LogonAuth { get; set; }

        /// <summary>
        /// Gets or sets information about the current two step authenication process.
        /// </summary>
        public TwoStepToken TwoStepToken { get; set; }

        /// <summary>
        /// Gets or sets the session values.
        /// </summary>
        public Session Session { get; set; }

        /// <summary>
        /// Gets a value indicating whether to force retrieving data from backend.
        /// </summary>
        public bool IsForceFromBackend { get; private set; }

        /// <summary>
        /// Gets or sets the csrf tokens.
        /// </summary>
        public IEnumerable<CsrfToken> CsrfTokens 
        {
            get 
            {
                foreach (CsrfClass csrfClass in Enum.GetValues(typeof(CsrfClass)))
                {
                    if (this.TryGetValue(out CsrfToken csrfToken, csrfClass))
                        yield return csrfToken;
                }
            }
            set 
            {
                foreach (CsrfClass csrfClass in Enum.GetValues(typeof(CsrfClass)))
                    this.DeleteValue<CsrfToken>(csrfClass);

                if (value != null)
                {
                    foreach (var csrfToken in value)
                        this.AddValue(csrfToken, csrfToken.Class);
                }
            }
        }

        /// <summary>
        /// Returns an context that will retrieve all data from the backend.
        /// </summary>
        /// <returns>The backend context.</returns>
        public ClientContext AsBackendContext()
        {
            return this.IsForceFromBackend ? this : new ClientContext(_stash)
            {
                Authentication = this.Authentication,
                AuthToken = this.AuthToken,
                LogonAuth = this.LogonAuth,
                TwoStepToken = this.TwoStepToken,
                Session = this.Session,
                IsForceFromBackend = true
            };
        }

        /// <summary>
        /// Returns an context that will check the cache first before retrieving 
        /// data from the backend.
        /// </summary>
        /// <returns>The backend context.</returns>
        public ClientContext AsCacheContext()
        {
            return !this.IsForceFromBackend ? this : new ClientContext(_stash)
            {
                Authentication = this.Authentication,
                AuthToken = this.AuthToken,
                LogonAuth = this.LogonAuth,
                TwoStepToken = this.TwoStepToken,
                Session = this.Session,
                IsForceFromBackend = false
            };
        }

        internal void AddValue<T>(T item, params object[] keys)
        {
            var lookupKey = CreateLookupKey<T>(keys);
            _stash.AddOrUpdate(lookupKey, item, (n, v) => item);
        }

        internal bool DeleteValue<T>(params object[] keys)
        {
            var lookupKey = CreateLookupKey<T>(keys);
            return _stash.TryRemove(lookupKey, out object unused);
        }

        internal bool TryGetValue<T>(out T item, params object[] keys)
        {
            var lookupKey = CreateLookupKey<T>(keys);
            if (_stash.TryGetValue(lookupKey, out object value))
            {
                item = (T)value;
                return true;
            }

            item = default(T);
            return false;
        }

        private static string CreateLookupKey<T>(params object[] keys)
        {
            var sb = new StringBuilder(typeof(T).FullName);
            foreach (var key in keys)
            {
                sb.Append("#");
                sb.Append(key);
            }
            return sb.ToString();
        }
    }
}
