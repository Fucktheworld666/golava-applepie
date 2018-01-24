using System.Threading.Tasks;

namespace GoLava.ApplePie.Security.CertificateStores
{
    public class CertificateStoreProxy : ICertificateStoreProxy
    {
        private readonly ICertificateStore _certificateStore;
        private readonly IKeychain _keychain;

        public CertificateStoreProxy(ICertificateStore certificateStore, IKeychain keychain)
        {
            _certificateStore = certificateStore;
            _keychain = keychain;
        }

        public Task DeleteAsync(string id)
        {
            return _certificateStore.DeleteAsync(id);
        }

        public Task<string> RetrievePrivateKeyAsync(string id)
        {
            return _certificateStore.RetrievePrivateKeyAsync(id);
        }

        public Task StorePrivateKeyAsync(string id, string privateKey)
        {
            return _certificateStore.StorePrivateKeyAsync(id, privateKey);
        }
    }
}
