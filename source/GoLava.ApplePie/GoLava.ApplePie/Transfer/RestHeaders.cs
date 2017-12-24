using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace GoLava.ApplePie.Transfer
{
    public class RestHeaders : Dictionary<string, HashSet<string>>
    {
        public RestHeaders()
            : base(StringComparer.InvariantCultureIgnoreCase) { }

        public RestHeaders(HttpHeaders headers)
           : this() 
        {
            foreach (var header in headers)
                this.Add(header.Key, new HashSet<string>(header.Value));
        }

        public void Add(string key, string value)
        {
            if (!this.TryGetValue(key, out HashSet<string> hashSet))
            {
                hashSet = new HashSet<string>();
                this.Add(key, hashSet);
            }
            hashSet.Add(value);
        }

        public void Add(string key, params string[] values)
        {
            if (!this.TryGetValue(key, out HashSet<string> hashSet))
            {
                hashSet = new HashSet<string>();
                this.Add(key, hashSet);
            }
            foreach (var value in values)
                hashSet.Add(value);
        }
    }
}