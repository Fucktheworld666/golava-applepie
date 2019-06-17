using System;
using System.Threading.Tasks;

namespace GoLava.ApplePie.Security.CertificateStores
{
    public interface ICertificateStore
    {
        Task DeleteAsync(string id);

        Task<PrivateCertificate> RetrievePrivateCertificateAsync(string id);

        Task StorePrivateCertificateAsync(string id, PrivateCertificate privateCertificate);
    }
}
