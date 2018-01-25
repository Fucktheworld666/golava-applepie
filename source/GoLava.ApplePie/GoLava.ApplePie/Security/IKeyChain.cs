using System.Threading.Tasks;

namespace GoLava.ApplePie.Security
{
    public interface IKeychain
    {
        Task<bool> ImportBinaryDataAsync(byte[] certificateData, string certificatePassword);

        Task<bool> ImportBinaryDataAsync(byte[] certificateData, string certificatePassword, string keychainFile, string keychainPassword);

        Task<bool> ImportFileAsync(string certificateFile, string certificatePassword);

        Task<bool> ImportFileAsync(string certificateFile, string certificatePassword, string keychainFile, string keychainPassword);
    }
}
