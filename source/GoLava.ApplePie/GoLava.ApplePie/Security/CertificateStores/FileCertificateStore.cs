using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GoLava.ApplePie.Exceptions;
using GoLava.ApplePie.Threading;

namespace GoLava.ApplePie.Security.CertificateStores
{
    public class FileCertificateStore : ICertificateStore
    {
        private const string PrivateKeyFileExtension = ".p8";

        public async Task DeleteAsync(string id)
        {
            await Configure.AwaitFalse();

            this.DeleteFile(id, PrivateKeyFileExtension);

            await Task.CompletedTask;
        }

        public async Task<string> RetrievePrivateKeyAsync(string id)
        {
            await Configure.AwaitFalse();

            try
            {
                var filePath = this.GetFile(id, PrivateKeyFileExtension);
                using (var reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    var privateKey = await reader.ReadToEndAsync();
                    return privateKey;
                }
            }
            catch (Exception ex)
            {
                throw new ApplePieCertificateStoreException(
                    $"Failed to retrieve private key with id '{id}'. See inner exception for more details.", ex);
            }
        }

        public async Task StorePrivateKeyAsync(string id, string privateKey)
        {
            await Configure.AwaitFalse();

            try
            {

                var filePath = this.GetFile(id, PrivateKeyFileExtension);
                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    await writer.WriteAsync(privateKey);
                }
            }
            catch (Exception ex)
            {
                throw new ApplePieCertificateStoreException(
                    $"Failed to store private key with id '{id}'. See inner exception for more details.", ex);
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