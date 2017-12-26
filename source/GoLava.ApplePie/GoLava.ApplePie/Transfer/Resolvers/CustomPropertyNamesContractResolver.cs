using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using GoLava.ApplePie.Contracts.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GoLava.ApplePie.Transfer.Resolvers
{
    /// <summary>
    /// A custom property names contract resolver that 
    /// uses a <see cref="T:CamelCasePropertyNamesContractResolver"/> as a base class.
    /// </summary>
    public class CustomPropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            if (member.GetCustomAttribute<JsonDataClassPropertyAttribute>() != null)
            {
                var type = jsonProperty.PropertyType;
                var attribute = FindJsonDataClassPropertyAttribute(type, out Type outType);
                if (outType == null && attribute == null)
                {
                    outType = type;
                    while (outType.IsGenericType || outType.IsArray)
                    {
                        outType = outType.IsGenericType 
                            ? outType.GenericTypeArguments[0] 
                            : outType.GetElementType();
                    }
                }
                var name = attribute?.Name ?? (outType ?? type).Name;
                jsonProperty.PropertyName = typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string)
                    ? Pluralize(name) : name;
            }
            return jsonProperty;
        }

        private static JsonDataPropertyAttribute FindJsonDataClassPropertyAttribute(Type type, out Type outType)
        {
            var attribute = type.GetCustomAttribute<JsonDataPropertyAttribute>();
            if (attribute == null)
            {
                outType = null;
                if (type.IsGenericType)
                {
                    var itemType = type.GetGenericArguments().First();
                    attribute = FindJsonDataClassPropertyAttribute(itemType, out outType);
                }
            }
            else 
            {
                outType = type;    
            }
            return attribute;
        }

        private static readonly string[] IesSuffixes = { "y", "ay", "ey", "oy", "uy" };

        private static string Pluralize(string name)
        {
            if (name.EndsWith(IesSuffixes[0], StringComparison.InvariantCulture))
            {
                if (!IesSuffixes.Skip(1).Any(x => name.EndsWith(x, StringComparison.InvariantCulture)))
                    return name.Substring(0, name.Length - 1) + "ies";
            }
                
            if (name.EndsWith("s", StringComparison.InvariantCulture))
                return name + "es";

            return name + "s";
        } 
    }
}