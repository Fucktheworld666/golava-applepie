using System;
namespace GoLava.ApplePie.Exceptions
{
    public class ApplePieCertificateStoreException : ApplePieException
    {
        public ApplePieCertificateStoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}