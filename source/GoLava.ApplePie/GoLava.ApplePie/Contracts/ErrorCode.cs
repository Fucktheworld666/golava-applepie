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

        public ErrorCode(int errorCode)
        {
            _errorCode = errorCode;
            _errorCodeString = _errorCode.ToString(CultureInfo.InvariantCulture);
        }

        public ErrorCode(string errorCode)
        {
            _errorCode = int.Parse(errorCode, CultureInfo.InvariantCulture);
            _errorCodeString = errorCode;
        }

        public override int GetHashCode()
        {
            return _errorCode.GetHashCode();
        }

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

        public static bool operator !=(ErrorCode e1, ErrorCode e2) => e1._errorCode != e2._errorCode;

        public static bool operator ==(ErrorCode e1, ErrorCode e2) => e1._errorCode == e2._errorCode;

        public static bool operator !=(ErrorCode errorCode, int i) => errorCode._errorCode != i;

        public static bool operator ==(ErrorCode errorCode, int i) => errorCode._errorCode == i;

        public static bool operator !=(int i, ErrorCode errorCode) => errorCode._errorCode != i;

        public static bool operator ==(int i, ErrorCode errorCode) => errorCode._errorCode == i;

        public static bool operator ==(ErrorCode errorCode, string s) => errorCode._errorCodeString == s;

        public static bool operator !=(ErrorCode errorCode, string s) => errorCode._errorCodeString != s;

        public static bool operator ==(string s, ErrorCode errorCode) => errorCode._errorCodeString == s;

        public static bool operator !=(string s, ErrorCode errorCode) => errorCode._errorCodeString != s;
    }
}
