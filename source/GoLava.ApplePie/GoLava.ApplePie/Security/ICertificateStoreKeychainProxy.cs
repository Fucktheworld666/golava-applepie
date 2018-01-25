using System.Threading.Tasks;

namespace GoLava.ApplePie.Security
{
    public interface ICertificateStoreKeychainProxy 
    {
        Task DeleteAsync(string id);

        Task<PrivateCertificate> RetrievePrivateCertificateAsync(string id);

        Task StorePrivateCertificateAsync(string id, PrivateCertificate privateCertificate, char[] privateCertificatePassword);
    }
}
