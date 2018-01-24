using System;
using System.Threading.Tasks;

namespace GoLava.ApplePie.Security
{
    public interface IKeychain
    {
        Task<bool> ImportAsync(string path, string keychainPath, string keychainPassword = "", string certificatePassword = "");
    }
}
