﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using GoLava.ApplePie.Extensions;
using GoLava.ApplePie.Transfer.Resolvers;

namespace GoLava.ApplePie.Transfer.Content
{
    public class CustomFormUrlEncodedContent : FormUrlEncodedContent
    {
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
                var name = ConvertName(property.Name);
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
                return string.Empty;
                    
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