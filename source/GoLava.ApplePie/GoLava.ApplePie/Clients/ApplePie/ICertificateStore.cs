using System;
using System.Threading.Tasks;

namespace GoLava.ApplePie.Clients.ApplePie
{
    public interface ICertificateStore
    {
        Task StoreAsync(string id, string privateKey);
    }
}
