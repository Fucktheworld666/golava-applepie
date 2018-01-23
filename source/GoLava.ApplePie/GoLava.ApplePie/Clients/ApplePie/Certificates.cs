using System.Security;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts.AppleDeveloper;
using GoLava.ApplePie.Security;
using GoLava.ApplePie.Threading;

namespace GoLava.ApplePie.Clients.ApplePie
{
    public sealed class Certificates
    {
        private readonly ApplePieClient _applePieClient;
        private readonly CertificateRequestFactory _certificateRequestFactory;

        private Certificates()
        {
            _certificateRequestFactory = new CertificateRequestFactory();
        }
        
        internal Certificates(ApplePieClient applePieClient)
            : this()
        {
            _applePieClient = applePieClient;
        }

        public CertificateRequestWithPrivateKey CreateCertificateRequestWithPrivateKey(SecureString securePassword)
        {
            return _certificateRequestFactory.CreateCertificateRequestWithPrivateKey(securePassword);
        }

        public async Task<CertificateRequest> CreateAndSubmitCertificateSigningRequestAsync(ClientContext clientContext, Application application, CertificateTypeDisplayId certificateType, SecureString securePassword)
        {
            await Configure.AwaitFalse();

            var certificateRequestWithPrivateKey = this.CreateCertificateRequestWithPrivateKey(securePassword);
            var result = await _applePieClient.AppleDeveloperClient.SubmitCertificateSigningRequestAsync(
                clientContext, 
                application.TeamId, 
                new CertificateSigningRequest
                {
                    Content = certificateRequestWithPrivateKey.CertificateRequest,
                    ApplicationId = application.Id,
                    Type = certificateType
                },
                application.Platform
            );
            await _applePieClient.CertificateStore.StoreAsync(result.CertificateId, certificateRequestWithPrivateKey.PrivateKey);
            return result;
        }
    }
}
