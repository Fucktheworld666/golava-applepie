using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GoLava.ApplePie.Extensions
{
    public static class StringExtensions
    {
        private static readonly Action<string, int, char> _setChar;
        private static readonly Action<string, int> _setLength;

        static StringExtensions()
        {
            // detect internal string operation functions
            if (Environment.Version.Major < 4)
            {
                var setCharMethod = typeof(string).GetMethod(
                    "SetChar",
                    BindingFlags.Instance | BindingFlags.NonPublic
                );
                _setChar = (Action<string, int, char>)Delegate.CreateDelegate(typeof(Action<string, int, char>), setCharMethod);

                var setLengthMethod = typeof(string).GetMethod(
                    "SetLength",
                    BindingFlags.Instance | BindingFlags.NonPublic
                );
                _setLength = (Action<string, int>)Delegate.CreateDelegate(typeof(Action<string, int>), setLengthMethod);
            }
            else
            {
                var fillStringCheckedMethod = typeof(string).GetMethod(
                    "FillStringChecked",
                    BindingFlags.Static | BindingFlags.NonPublic
                );

                var fillStringCheckedDelegate = (Action<string, int, string>)Delegate.CreateDelegate(
                    typeof(Action<string, int, string>), fillStringCheckedMethod
                );
                _setChar = (str, i, c) => fillStringCheckedDelegate(str, i, c.ToString());

                var stringLengthField = typeof(string).GetField(
                    "m_stringLength",
                    BindingFlags.Instance | BindingFlags.NonPublic
                );

                var input = Expression.Parameter(typeof(string), "input");
                var length = Expression.Parameter(typeof(int), "length");

                var setLengthLambda = Expression.Lambda<Action<string, int>>(
                    Expression.Assign(Expression.Field(input, stringLengthField), length),
                    input,
                    length
                );

                _setLength = setLengthLambda.Compile();
            }
        }

        /// <summary>
        /// Changes the string in place.
        /// </summary>
        public static void ChangeTo(this string s, string value)
        {
            _setLength(s, value.Length);
            for (int i = 0; i < value.Length; ++i)
                s.SetChar(i, value[i]);
        }

        /// <summary>
        /// Sets the chararacter at the given position
        /// </summary>
        public static void SetChar(this string s, int position, char value)
        {
            _setChar(s, position, value);
        }

        /// <summary>
        /// Clears the string in place.
        /// </summary>
        public static void Clear(this string s)
        {
            s.ChangeTo(new string(' ', s.Length));
            _setLength(s, 0);
        }

        /// <summary>
        /// Escapes all characters of the given string so it can be safely used as a parameter in a shell.
        /// </summary>
        /// <param name="s">The string to be escaped.</param>
        public static string ShellEscape(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return "''";

            s = Regex.Replace(s, @"([^A-Za-z0-9_\-.,:/@\n])", @"\$1");
            s = Regex.Replace(s, @"\n", @"'\n'");

            return s;
        }
    }
}
