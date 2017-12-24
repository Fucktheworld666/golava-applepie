using System;
namespace GoLava.ApplePie.Exceptions
{
    public class ApplePieException : Exception
    {
        public ApplePieException(string message)
            : base(message) { }

        public ApplePieException(string message, Exception exception)
            : base(message, exception) { }
    }
}