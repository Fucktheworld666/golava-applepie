using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GoLava.ApplePie.Clients.AppleDeveloper;
using GoLava.ApplePie.Contracts.AppleDeveloper;
using GoLava.ApplePie.Exceptions;
using GoLava.ApplePie.Security;
using GoLava.ApplePie.Security.CertificateStores;
using GoLava.ApplePie.Threading;

namespace GoLava.ApplePie.Clients.ApplePie
{
    public sealed class Certificates : ICertificates
    {
        private readonly IAppleDeveloperClient _appleDeveloperClient;
        private readonly ICertificateStoreProxy _certificateStoreProxy;
        private readonly CertificateRequestFactory _certificateRequestFactory;
        
        public Certificates(
            IAppleDeveloperClient appleDeveloperClient,
            ICertificateStoreProxy cerificateStoreProxy)
        {
            _appleDeveloperClient = appleDeveloperClient;
            _certificateStoreProxy = cerificateStoreProxy;
            _certificateRequestFactory = new CertificateRequestFactory();
        }

        public async Task<X509Certificate2> CreateCertificateAsync(ClientContext clientContext, Application application, CertificateTypeDisplayId certificateType, SecureString securePassword)
        {
            await Configure.AwaitFalse();

            X509Certificate2 certificate = null;
            try
            {

                var certificateRequestWithPrivateKey = _certificateRequestFactory.CreateCertificateRequestWithPrivateKey();
                var certificateRequest = await _appleDeveloperClient.SubmitCertificateSigningRequestAsync(
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

                certificate = await _appleDeveloperClient.DownloadCertificateAsync(
                    clientContext, 
                    certificateRequest);

                await _certificateStoreProxy.StorePrivateKeyAsync(
                    certificate.Thumbprint, 
                    certificateRequestWithPrivateKey.PrivateKey);


                /*
                if (_applePieClient.EnvironmentDetector.IsMac)
                {
          //          var importResult = await _applePieClient.Keychain.ImportAsync("", ".");
                }*/

                return certificate;
            }
            catch (ApplePieCertificateStoreException)
            {
                // we need to rollback
                // await _appleDeveloperClient.RevokeCertificateRequestAsync(clientContext, certificateRequest);
                throw;
            }
        }
    }
}
