using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GoLava.ApplePie.Exceptions;
using GoLava.ApplePie.Threading;

namespace GoLava.ApplePie.Security.CertificateStores
{
    public class FileCertificateStore : ICertificateStore
    {
        private const string PrivateCertificateFileExtension = ".p12";

        public async Task DeleteAsync(string id)
        {
            await Configure.AwaitFalse();

            this.DeleteFile(id, PrivateCertificateFileExtension);

            await Task.CompletedTask;
        }

        public async Task<PrivateCertificate> RetrievePrivateCertificateAsync(string id)
        {
            await Configure.AwaitFalse();

            try
            {
                var filePath = this.GetFile(id, PrivateCertificateFileExtension);

                using (var ms = new MemoryStream())
                {
                    var buffer = new byte[4096];
                    using (var file = File.OpenRead(filePath))
                    {
                        var read = await file.ReadAsync(buffer, 0, buffer.Length);
                        while (read > 0)
                        {
                            await ms.WriteAsync(buffer, 0, read);
                            read = await file.ReadAsync(buffer, 0, buffer.Length);
                        }
                    }

                    return new PrivateCertificate
                    {
                        Pkcs12 = ms.ToArray()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new ApplePieCertificateStoreException(
                    $"Failed to retrieve private certificate with id '{id}'. See inner exception for more details.", ex);
            }
        }

        public async Task StorePrivateCertificateAsync(string id, PrivateCertificate privateCertificate)
        {
            await Configure.AwaitFalse();

            try
            {
                var filePath = this.GetFile(id, PrivateCertificateFileExtension);
                using (var file = File.OpenWrite(filePath))
                {
                    var pkcs12 = privateCertificate.Pkcs12;
                    await file.WriteAsync(pkcs12, 0, pkcs12.Length);
                }
            }
            catch (Exception ex)
            {
                throw new ApplePieCertificateStoreException(
                    $"Failed to store private certificate with id '{id}'. See inner exception for more details.", ex);
            }
        }

        private void DeleteFile(string id, string fileExtension)
        {
            var file = GetFile(id, fileExtension);
            File.Delete(file);
        }

        private string GetFile(string id, string fileExtension)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "certificates");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Path.Combine(path, $"{id}{fileExtension}");
        }
    }
}