using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GoLava.ApplePie.Threading;

namespace GoLava.ApplePie.Clients.ApplePie.CertificateStores
{
    public class FileCertificateStore : ICertificateStore
    {
        public async Task StoreAsync(string id, string privateKey)
        {
            await Configure.AwaitFalse();

            var path = Path.Combine(Environment.CurrentDirectory, "certificates");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var p8File = Path.Combine(path, $"{id}.p8");
            using (var writer = new StreamWriter(p8File, false, Encoding.UTF8))
            {
                await writer.WriteAsync(privateKey);
            }
        }
    }
}