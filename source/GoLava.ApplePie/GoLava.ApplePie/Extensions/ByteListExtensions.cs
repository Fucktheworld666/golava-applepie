using System.Collections.Generic;

namespace GoLava.ApplePie.Extensions
{
    public static class ByteListExtensions
    {
        private static char[] hexLookup = new [] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        public static char[] ToHexCharArray(this IList<byte> bytes)
        {
            var length = bytes.Count - 1;
            var c = new char[length * 2 + 2];
            int i = -1, p = -1;

            byte b;
            while (i < length)
            {
                b = bytes[++i];
                c[++p] = hexLookup[b >> 4];
                c[++p] = hexLookup[b & 0xF];
            }
            return c;
        }

        public static string ToHexString(this IList<byte> bytes)
        {
            var hex = bytes.ToHexCharArray();
            return new string(hex, 0, hex.Length);
        }
    }
}
