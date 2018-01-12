using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using GoLava.ApplePie.Extensions;

namespace GoLava.ApplePie.Formatting
{
    /// <summary>
    /// A formatter for named parameters within strings.
    /// </summary>
    public class NamedFormatter
    {
        private static readonly Regex RegexFormatPlaceHolder = new Regex(@"(?<=[^\{]\{|\{)[^{}]+?(?=:|\})", RegexOptions.Compiled);
        private static readonly MethodInfo FormatMethod = typeof(string).GetMethod("Format", new[] { typeof(string), typeof(object[]) });
        private static readonly MethodInfo ConvertMethod = typeof(NamedFormatter).GetMethod("Convert", BindingFlags.NonPublic | BindingFlags.Static);
        private readonly ConcurrentDictionary<string, Func<object, string>> _preCompiledExpressions;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NamedFormatter"/> class.
        /// </summary>
        /// <param name="preCompiledExpressions">A collection of precompiled expressions.</param>
        public NamedFormatter(ConcurrentDictionary<string, Func<object, string>> preCompiledExpressions = null)
        {
            _preCompiledExpressions = preCompiledExpressions
                ?? new ConcurrentDictionary<string, Func<object, string>>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Formats a string by replacing placeholders with a string pattern with the value of properties of the
        /// given item with the same name. 
        /// </summary>
        /// <param name="pattern">The pattern the holds the placeholders to be replaced.</param>
        /// <param name="item">The object that has properties matching the placeholders of the pattern.</param>
        /// <returns>A string that is the result of applying the item with the given pattern.</returns>
        /// <example>
        /// var result = namedFormatter.Format("Hello {name}!", new { name = "World" });
        /// // result has a value of "Hello World!"
        /// </example>
        public string Format(string pattern, object item)
        {
            if (item == null)
                return pattern;

            var inputType = item.GetType();
            var lookupKey = pattern + inputType.FullName;

            if (_preCompiledExpressions.TryGetValue(lookupKey, out Func<object, string> func) && func != null)
                return func(item);
            
            var arguments = ParsePattern(pattern, out string replacedPattern, item);

            var patternExpression = Expression.Constant(replacedPattern, typeof(string));
            var parameterExpression = Expression.Parameter(typeof(object));
            var convertedInput = Expression.Convert(parameterExpression, inputType);
            var argumentArrayElements = arguments.Select(argument => Expression.Call(null, ConvertMethod, Expression.Convert(Expression.PropertyOrField(convertedInput, argument), typeof(object))));
            var argumentArrayExpressions = Expression.NewArrayInit(typeof(object), argumentArrayElements);
            var formatCallExpression = Expression.Call(FormatMethod, patternExpression, argumentArrayExpressions);
            var lambdaExpression = Expression.Lambda<Func<object, string>>(formatCallExpression, parameterExpression);

            // The lambda expression will look something like this input => string.Format("my format
            // string", new[]{ input.Arg0, input.Arg1, ... });

            func = lambdaExpression.Compile();

            _preCompiledExpressions.TryAdd(lookupKey, func);

            return func(item);
        }

        private static object Convert(object o)
        {
            if (o is Enum e)
                return e.ToStringValue();
            return o;
        }

        private static IEnumerable<string> ParsePattern(string pattern, out string replacedPattern, object item)
        {
            var sb = new StringBuilder();
            var lastIndex = 0;
            var arguments = new List<string>();
            var lowerArguments = new List<string>();

            var itemType = item.GetType();
            foreach (var match in RegexFormatPlaceHolder.Matches(pattern).Cast<Match>())
            {
                var key = match.Value;
                var property = itemType.GetProperty(key);
                if (property != null)
                {
                    var lowerKey = key.ToLowerInvariant();
                    var index = lowerArguments.IndexOf(lowerKey);
                    if (index < 0)
                    {
                        index = lowerArguments.Count;
                        lowerArguments.Add(lowerKey);
                        arguments.Add(key);
                    }

                    sb.Append(pattern.Substring(lastIndex, match.Index - lastIndex));
                    sb.Append(index);
                    if (property.PropertyType == typeof(DateTime))
                        sb.Append(":o");
                    lastIndex = match.Index + match.Length;
                } 
                else 
                {
                    sb.Append(pattern.Substring(lastIndex, match.Index - lastIndex - 1));
                    lastIndex = match.Index + match.Length + 1;
                }
            }

            sb.Append(pattern.Substring(lastIndex));
            replacedPattern = sb.ToString();
            return arguments;
        }
    }
}
