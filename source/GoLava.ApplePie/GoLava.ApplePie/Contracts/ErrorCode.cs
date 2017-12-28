using System.Globalization;

namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// A struct that represents an error code.
    /// </summary>
    public struct ErrorCode
    {
        public static readonly ErrorCode IncorrectVerificationCode = new ErrorCode(-21669);
        public static readonly ErrorCode Ok = new ErrorCode(0);
        public static readonly ErrorCode Unknown = new ErrorCode(int.MinValue);

        private readonly int _errorCode;
        private readonly string _errorCodeString;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ErrorCode"/> struct.
        /// </summary>
        /// <param name="errorCode">The error code as a int value.</param>
        public ErrorCode(int errorCode)
        {
            _errorCode = errorCode;
            _errorCodeString = _errorCode.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ErrorCode"/> struct.
        /// </summary>
        /// <param name="errorCode">The error code as a string value.</param>
        public ErrorCode(string errorCode)
        {
            _errorCode = int.Parse(errorCode, CultureInfo.InvariantCulture);
            _errorCodeString = errorCode;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:ErrorCode"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in 
        /// hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return _errorCode.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:GoLava.ApplePie.Contracts.ErrorCode"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:GoLava.ApplePie.Contracts.ErrorCode"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:GoLava.ApplePie.Contracts.ErrorCode"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is int i)
                return this == i;

            if (obj is string s)
                return this == s;

            if (obj is ErrorCode e)
                return this == e;

            return false;
        }

        /// <summary>
        /// Determines whether a specified instance of <see cref="ErrorCode"/> is not equal to
        /// another specified <see cref="ErrorCode"/>.
        /// </summary>
        /// <param name="e1">The first <see cref="ErrorCode"/> to compare.</param>
        /// <param name="e2">The second <see cref="ErrorCode"/> to compare.</param>
        /// <returns><c>true</c> if <c>e1</c> and <c>e2</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ErrorCode e1, ErrorCode e2) => e1._errorCode != e2._errorCode;

        /// <summary>
        /// Determines whether a specified instance of <see cref="ErrorCode"/> is equal to
        /// another specified <see cref="ErrorCode"/>.
        /// </summary>
        /// <param name="e1">The first <see cref="ErrorCode"/> to compare.</param>
        /// <param name="e2">The second <see cref="ErrorCode"/> to compare.</param>
        /// <returns><c>true</c> if <c>e1</c> and <c>e2</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ErrorCode e1, ErrorCode e2) => e1._errorCode == e2._errorCode;

        /// <summary>
        /// Determines whether a specified instance of <see cref="ErrorCode"/> is not equal to
        /// another specified <see cref="int"/>.
        /// </summary>
        /// <param name="errorCode">The <see cref="ErrorCode"/> to compare.</param>
        /// <param name="i">The <see cref="int"/> to compare.</param>
        /// <returns><c>true</c> if <c>errorCode</c> and <c>i</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ErrorCode errorCode, int i) => errorCode._errorCode != i;

        /// <summary>
        /// Determines whether a specified instance of <see cref="ErrorCode"/> is equal to
        /// another specified <see cref="int"/>.
        /// </summary>
        /// <param name="errorCode">The <see cref="ErrorCode"/> to compare.</param>
        /// <param name="i">The <see cref="int"/> to compare.</param>
        /// <returns><c>true</c> if <c>errorCode</c> and <c>i</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ErrorCode errorCode, int i) => errorCode._errorCode == i;

        /// <summary>
        /// Determines whether a specified instance of <see cref="int"/> is not equal to another specified <see cref="ErrorCode"/>.
        /// </summary>
        /// <param name="i">The <see cref="int"/> to compare.</param>
        /// <param name="errorCode">The <see cref="ErrorCode"/> to compare.</param>
        /// <returns><c>true</c> if <c>i</c> and <c>errorCode</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(int i, ErrorCode errorCode) => errorCode._errorCode != i;

        /// <summary>
        /// Determines whether a specified instance of <see cref="int"/> is equal to another specified <see cref="ErrorCode"/>.
        /// </summary>
        /// <param name="i">The <see cref="int"/> to compare.</param>
        /// <param name="errorCode">The <see cref="ErrorCode"/> to compare.</param>
        /// <returns><c>true</c> if <c>i</c> and <c>errorCode</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(int i, ErrorCode errorCode) => errorCode._errorCode == i;

        /// <summary>
        /// Determines whether a specified instance of <see cref="ErrorCode"/> is equal to
        /// another specified <see cref="string"/>.
        /// </summary>
        /// <param name="errorCode">The <see cref="ErrorCode"/> to compare.</param>
        /// <param name="s">The <see cref="string"/> to compare.</param>
        /// <returns><c>true</c> if <c>errorCode</c> and <c>s</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ErrorCode errorCode, string s) => errorCode._errorCodeString == s;

        /// <summary>
        /// Determines whether a specified instance of <see cref="ErrorCode"/> is not equal to
        /// another specified <see cref="string"/>.
        /// </summary>
        /// <param name="errorCode">The <see cref="ErrorCode"/> to compare.</param>
        /// <param name="s">The <see cref="string"/> to compare.</param>
        /// <returns><c>true</c> if <c>errorCode</c> and <c>s</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ErrorCode errorCode, string s) => errorCode._errorCodeString != s;

        /// <summary>
        /// Determines whether a specified instance of <see cref="string"/> is equal to another specified <see cref="ErrorCode"/>.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to compare.</param>
        /// <param name="errorCode">The <see cref="ErrorCode"/> to compare.</param>
        /// <returns><c>true</c> if <c>s</c> and <c>errorCode</c> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(string s, ErrorCode errorCode) => errorCode._errorCodeString == s;

        /// <summary>
        /// Determines whether a specified instance of <see cref="string"/> is not equal to another specified <see cref="ErrorCode"/>.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to compare.</param>
        /// <param name="errorCode">The <see cref="ErrorCode"/> to compare.</param>
        /// <returns><c>true</c> if <c>s</c> and <c>errorCode</c> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(string s, ErrorCode errorCode) => errorCode._errorCodeString != s;
    }
}