using System;
using System.Security;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts.AppleDeveloper;

namespace GoLava.ApplePie.Clients.ApplePie
{
    public static class ApplicationExtensions
    {
        private static ApplePieClient _appliePieClient;

        static ApplicationExtensions()
        {
            _appliePieClient = new ApplePieClient();
        }

        public static Task<CertificateRequest> CreateAndSubmitCertificateSigningRequestAsync(
            this Application application, ClientContext clientContext, 
            SecureString securePassword, CertificateTypeDisplayId certificateType)
        {
            return _appliePieClient.Certificates.CreateAndSubmitCertificateSigningRequestAsync(
                clientContext, securePassword, certificateType, application
            );
        }
    }
}
