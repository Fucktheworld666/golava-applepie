using System;
using System.Threading.Tasks;

namespace GoLava.ApplePie.Security.CertificateStores
{
    public interface ICertificateStore
    {
        Task DeleteAsync(string id);

        Task<string> RetrievePrivateKeyAsync(string id);

        Task StorePrivateKeyAsync(string id, string privateKey);
    }
}
