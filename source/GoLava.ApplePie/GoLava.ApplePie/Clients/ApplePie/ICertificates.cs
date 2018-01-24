using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts.AppleDeveloper;

namespace GoLava.ApplePie.Clients.ApplePie
{
    public interface ICertificates
    {
        Task<X509Certificate2> CreateCertificateAsync(
            ClientContext clientContext, 
            Application application, 
            CertificateTypeDisplayId certificateType, 
            SecureString securePassword);
    }
}