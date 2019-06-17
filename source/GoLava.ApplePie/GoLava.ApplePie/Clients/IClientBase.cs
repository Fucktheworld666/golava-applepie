using System.Security;
using System.Threading.Tasks;
using GoLava.ApplePie.Contracts;

namespace GoLava.ApplePie.Clients
{
    public interface IClientBase
    {
        Task<ClientContext> AcquireTwoStepCodeAsync(ClientContext context, TrustedDevice trustedDevice);

        Task<ClientContext> LogonWithCredentialsAsync(string username, string password);

        Task<ClientContext> LogonWithCredentialsAsync(string username, SecureString password);

        Task<ClientContext> LogonWithCredentialsAsync(Credentials credentials);

        Task<ClientContext> LogonWithTwoStepCodeAsync(ClientContext context, string code);
    }
}