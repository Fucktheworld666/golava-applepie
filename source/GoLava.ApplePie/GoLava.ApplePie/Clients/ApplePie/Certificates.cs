using System;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GoLava.ApplePie.Clients.AppleDeveloper;
using GoLava.ApplePie.Contracts.AppleDeveloper;
using GoLava.ApplePie.Exceptions;
using GoLava.ApplePie.Extensions;
using GoLava.ApplePie.Security;
using GoLava.ApplePie.Security.CertificateStores;
using GoLava.ApplePie.Threading;

namespace GoLava.ApplePie.Clients.ApplePie
{
    public sealed class Certificates : ICertificates
    {
        private readonly IAppleDeveloperClient _appleDeveloperClient;
        private readonly ICertificateStoreKeychainProxy _certificateStoreProxy;
        private readonly CertificateRequestCreator _certificateRequestCreator;
        private readonly PrivateCertificateCreator _privateCertificateCreator;
        
        public Certificates(
            IAppleDeveloperClient appleDeveloperClient,
            ICertificateStoreKeychainProxy cerificateStoreProxy)
        {
            _appleDeveloperClient = appleDeveloperClient;
            _certificateStoreProxy = cerificateStoreProxy;
            _certificateRequestCreator = new CertificateRequestCreator();
            _privateCertificateCreator = new PrivateCertificateCreator();
        }

        public async Task<X509Certificate2> CreateCertificateAsync(ClientContext clientContext, Application application, CertificateTypeDisplayId certificateType)
        {
            await Configure.AwaitFalse();

            var secureRandomProvider = new SecureRandomProvider();
            var secureRandom = secureRandomProvider.GetSecureRandom();

            var randomBytes = new byte[18];
            secureRandom.NextBytes(randomBytes);

            var password = randomBytes.ToHexCharArray();
            Array.Clear(randomBytes, 0, randomBytes.Length);

            try 
            {
                return await this.CreateCertificateAsync(clientContext, application, certificateType, password);
            }
            finally
            {
                Array.Clear(password, 0, password.Length);
            }
        }

        public async Task<X509Certificate2> CreateCertificateAsync(ClientContext clientContext, Application application, CertificateTypeDisplayId certificateType, SecureString securePassword)
        {
            await Configure.AwaitFalse();

            var secureStringConverter = new SecureStringConverter();
            var password = secureStringConverter.ConvertSecureStringToPlainCharArray(securePassword);
            try
            {
                return await this.CreateCertificateAsync(clientContext, application, certificateType, password);
            }
            finally
            {
                Array.Clear(password, 0, password.Length);
            }
        }

        public async Task<X509Certificate2> CreateCertificateAsync(ClientContext clientContext, Application application, CertificateTypeDisplayId certificateType, char[] password)
        {
            await Configure.AwaitFalse();

            X509Certificate2 certificate = null;
            CertificateRequest certificateRequest = null;
            try
            {
                var certificateRequestWithPrivateKey = _certificateRequestCreator.CreateCertificateRequestWithPrivateKey();

                certificateRequest = await _appleDeveloperClient.SubmitCertificateSigningRequestAsync(
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

                var pirvateCertificate = _privateCertificateCreator.CreatePrivateCertificate(
                    certificate, certificateRequestWithPrivateKey.PrivateKey,
                    password);
                                                    
                await _certificateStoreProxy.StorePrivateCertificateAsync(
                    certificate.Thumbprint,
                    pirvateCertificate,
                    password);

                return certificate;
            }
            catch (ApplePieCertificateStoreException)
            {
                // we need to rollback
                if (certificateRequest != null)
                    await _appleDeveloperClient.RevokeCertificateRequestAsync(clientContext, certificateRequest);
                throw;
            }
        }
    }
}
