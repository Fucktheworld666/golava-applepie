using System;
using System.Runtime.InteropServices;
using System.Security;

namespace GoLava.ApplePie.Security
{
    /// <summary>
    /// Secure string converter.
    /// </summary>
    public class SecureStringConverter
    {
        /// <summary>
        /// Converts a secure string to a plain string.
        /// </summary>
        /// <param name="secureString">The secure string to be converted.</param>
        public string ConvertSecureStringToPlainString(SecureString secureString)
        {
            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Converts a secure string to plain char array.
        /// </summary>
        /// <param name="secureString">The secure string to be converted.</param>
        public char[] ConvertSecureStringToPlainCharArray(SecureString secureString)
        {
            var unmanagedBstr = IntPtr.Zero;
            try
            {
                unmanagedBstr = Marshal.SecureStringToBSTR(secureString);

                var charArray = new char[secureString.Length];
                Marshal.Copy(unmanagedBstr, charArray, 0, charArray.Length);
                return charArray;
            }
            finally
            {
                Marshal.ZeroFreeBSTR(unmanagedBstr);
            }
        }
    }
}
