using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace GoLava.ApplePie.Extensions
{
    /// <summary>
    /// Extension methods on <see cref="T:Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts the given <see cref="T:Enum"/> to actual string value.
        /// </summary>
        /// <returns>The description string of the <see cref="T:Enum"/>.</returns>
        public static string ToStringValue(this Enum e)
        {
            var value = e.ToString();
            var fieldInfo = e.GetType().GetField(value);
            var attribute = fieldInfo.GetCustomAttribute<EnumMemberAttribute>();
            return attribute != null ? attribute.Value : value;
        }
    }
}