using System.Threading.Tasks;
using GoLava.ApplePie.Components;
using GoLava.ApplePie.Exceptions;
using GoLava.ApplePie.Extensions;
using GoLava.ApplePie.Security.CertificateStores;
using GoLava.ApplePie.Threading;

namespace GoLava.ApplePie.Security
{
    public class CertificateStoreKeychainProxy : ICertificateStoreKeychainProxy
    {
        private readonly ICertificateStore _certificateStore;
        private readonly IKeychain _keychain;
        private readonly IEnvironmentDetector _environmentDetector;

        public CertificateStoreKeychainProxy(
            ICertificateStore certificateStore, 
            IKeychain keychain, 
            IEnvironmentDetector environmentDetector)
        {
            _certificateStore = certificateStore;
            _keychain = keychain;
            _environmentDetector = environmentDetector;
        }

        public Task DeleteAsync(string id)
        {
            return _certificateStore.DeleteAsync(id);
        }

        public Task<PrivateCertificate> RetrievePrivateCertificateAsync(string id)
        {
            return _certificateStore.RetrievePrivateCertificateAsync(id);
        }

        public async Task StorePrivateCertificateAsync(string id, PrivateCertificate privateCertificate, char[] privateCertificatePassword)
        {
            await Configure.AwaitFalse();

            await _certificateStore.StorePrivateCertificateAsync(id, privateCertificate);
            if (!_environmentDetector.IsWindows)
            {
                var password = new string(privateCertificatePassword);
                try 
                {
                    var result = await _keychain.ImportBinaryDataAsync(privateCertificate.Pkcs12, password);
                    if (!result)
                    {
                        await _certificateStore.DeleteAsync(id);
                        throw new ApplePieCertificateStoreException($"Failed to store certificate with '{id}' into keychain.");
                    }
                }
                finally 
                {
                    password.Clear();
                }
            }
        }
    }
}
