using System;
using System.Text.RegularExpressions;

namespace GoLava.ApplePie.Extensions
{
    public static class StringExtensions
    {
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
