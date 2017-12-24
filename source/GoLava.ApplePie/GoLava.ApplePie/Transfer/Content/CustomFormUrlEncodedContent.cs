using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace GoLava.ApplePie.Transfer.Content
{
    public class CustomFormUrlEncodedContent : FormUrlEncodedContent
    {
        public CustomFormUrlEncodedContent(object content)
            : base(Convert(content)) { }

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
                var value = property.GetMethod.Invoke(content, null);
                list.Add(new KeyValuePair<string, string>(property.Name, value.ToString()));
            }

            return list;
        }
    }
}