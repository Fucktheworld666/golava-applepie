using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace GoLava.ApplePie.Transfer
{
    /// <summary>
    /// Class that stores request and response header key value pairs for rest
    /// calls.
    /// </summary>
    public class RestHeaders : IEnumerable<KeyValuePair<string, ISet<string>>>
    {
        private Dictionary<string, ISet<string>> _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RestHeaders"/> class.
        /// </summary>
        public RestHeaders()
        {
            _container = new Dictionary<string, ISet<string>>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RestHeaders"/> class.
        /// </summary>
        /// <param name="headers">The <see cref="HttpHeaders"/> instance to be added.</param>
        public RestHeaders(HttpHeaders headers)
           : this() 
        {
            if (headers != null)
            {
                foreach (var header in headers.Where(h => !string.IsNullOrEmpty(h.Key)))
                    _container.Add(header.Key, new HashSet<string>(header.Value));
            }
        }

        /// <summary>
        /// Adds value to the <see cref="T:RestHeaders"/> instance and stores it
        /// under the given key.
        /// </summary>
        /// <param name="key">The key to add a value to.</param>
        /// <param name="value">The value to be added to the key.</param>
        public void Add(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (!_container.TryGetValue(key, out ISet<string> hashSet))
            {
                hashSet = new HashSet<string>();
                _container.Add(key, hashSet);
            }
            hashSet.Add(value);
        }

        /// <summary>
        /// Adds one or more values to the <see cref="T:RestHeaders"/> instance
        /// and stores them under the given key.
        /// </summary>
        /// <param name="key">The key to add a value to.</param>
        /// <param name="values">The values to be added to the key.</param>
        public void Add(string key, params string[] values)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            
            if (!_container.TryGetValue(key, out ISet<string> hashSet))
            {
                hashSet = new HashSet<string>();
                _container.Add(key, hashSet);
            }
            foreach (var value in values)
                hashSet.Add(value);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An enumerator instance of type 
        /// <see cref="T:IEnumerator&lt;KeyValuePair&lt;string, ISet&lt;string&gt;&gt;&gt;"/>
        /// that allows enumeration of all key value pairs of this <see cref="T:RestHeaders"/> instance.</returns>
        public IEnumerator<KeyValuePair<string, ISet<string>>> GetEnumerator()
        {
            return _container.GetEnumerator();
        }

        /// <summary>
        /// Sets the values stored under the given key to the specified value.
        /// </summary>
        /// <returns>The set.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Set(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            _container.Remove(key);
            this.Add(key, value);
        }

        /// <summary>
        /// Sets the values stored under the given key to the specified values.
        /// </summary>
        /// <returns>The set.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Set(string key, params string[] value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            _container.Remove(key);
            this.Add(key, value);
        }

        /// <summary>
        /// Tries to retrieve a set of values stored under the given key 
        /// within this <see cref="T:RestHeaders"/> instance.
        /// </summary>
        /// <returns><c>true</c>, if set of values was successfully retrieved, <c>false</c> otherwise.</returns>
        /// <param name="key">The key that is used to retrieve the set of values.</param>
        /// <param name="values">A reference to the set of values.</param>
        public bool TryGetValue(string key, out ISet<string> values)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return _container.TryGetValue(key, out values);
        }

        /// <summary>
        /// Tries to retrieve a single value stored under the given key 
        /// within this <see cref="T:RestHeaders"/> instance.
        /// </summary>
        /// <returns><c>true</c>, if value was successfully retrieved, <c>false</c> otherwise.</returns>
        /// <param name="key">The key that is used to retrieve the value.</param>
        /// <param name="value">A reference to the value.</param>
        public bool TryGetValue(string key, out string value)
        {
            if (this.TryGetValue(key, out ISet<string> values) && values?.Count == 1)
            {
                value = values.First();
                return true;
            }

            value = default(string);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}