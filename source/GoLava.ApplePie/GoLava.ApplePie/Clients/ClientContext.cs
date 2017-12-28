using System.Collections.Concurrent;
using System.Text;
using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Transfer;

namespace GoLava.ApplePie.Clients
{
    public class ClientContext : RestClientContext
    {
        private readonly ConcurrentDictionary<string, object> _stash;

        public ClientContext()
        {
            _stash = new ConcurrentDictionary<string, object>();
        }

        public Authentication Authentication { get; set; }

        public AuthToken AuthToken { get; set; }

        public LogonAuth LogonAuth { get; set; }

        public TwoStepToken TwoStepToken { get; set; }

        public Session Session { get; set; }

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
