using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using GoLava.ApplePie.Contracts.Attributes;
using GoLava.ApplePie.Extensions;
using GoLava.ApplePie.Transfer.Resolvers;
using Newtonsoft.Json;

namespace GoLava.ApplePie.Transfer.Content
{
    /// <summary>
    /// A custom container for name/value tuples encoded using application/x-www-form-urlencoded MIME type.
    /// </summary>
    public class CustomFormUrlEncodedContent : FormUrlEncodedContent
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:CustomFormUrlEncodedContent"/> class.
        /// </summary>
        /// <param name="content">Properties of this object will be used as name/value pairs..</param>
        public CustomFormUrlEncodedContent(object content)
            : base(Convert(content)) { }

        private static CustomPropertyNamesContractResolver _resolver = new CustomPropertyNamesContractResolver();

        private static IEnumerable<KeyValuePair<string, string>> Convert(object content)
        {
            if (content == null)
                return Enumerable.Empty<KeyValuePair<string, string>>();

            if (content is IEnumerable<KeyValuePair<string, string>> enumerable)
                return enumerable;

            var list = new List<KeyValuePair<string, string>>();

            var parametersType = content.GetType();
            foreach (var property in parametersType.GetProperties())
            {
                var value = ConvertValue(property.GetMethod.Invoke(content, null));
                if (value == null)
                    continue;
                
                string name;

                var formDataPropertyAttribute = property.GetCustomAttribute<FormDataPropertyAttribute>();
                if (!string.IsNullOrEmpty(formDataPropertyAttribute?.PropertyName))
                {
                    name = formDataPropertyAttribute.PropertyName;
                }
                else 
                {
                    var jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyAttribute>();
                    if (!string.IsNullOrEmpty(jsonPropertyAttribute?.PropertyName))
                        name = jsonPropertyAttribute.PropertyName;
                    else
                        name = ConvertName(property.Name);
                }

                list.Add(new KeyValuePair<string, string>(name, value));
            }

            return list;
        }

        private static string ConvertName(string name)
        {
            return _resolver.GetResolvedPropertyName(name);
        }

        private static string ConvertValue(object value)
        {
            if (value == null)
                return null;
                    
            if (value is string s)
                return s;

            if (value is int i)
                return i.ToString(CultureInfo.InvariantCulture);

            if (value is long l)
                return l.ToString(CultureInfo.InvariantCulture);

            if (value is Enum e)
                return e.ToDescriptionString();
            
            if (value is IEnumerable collection)
            {
                var sb = new StringBuilder();
                foreach (var v in collection)
                {
                    if (sb.Length > 0)
                        sb.Append(",");
                    sb.Append(ConvertValue(v));
                }
                return sb.ToString();
            }

            return value.ToString();
        }
    }
}