using System;
using System.ComponentModel;
using System.Reflection;

namespace GoLava.ApplePie.Extensions
{
    /// <summary>
    /// Extension methods on <see cref="T:Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts the given <see cref="T:Enum"/> to its description string.
        /// </summary>
        /// <returns>The description string of the <see cref="T:Enum"/>.</returns>
        public static string ToDescriptionString(this Enum e)
        {
            var value = e.ToString();
            var fieldInfo = e.GetType().GetField(value);
            var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value;
        }
    }
}