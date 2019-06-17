using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GoLava.ApplePie.Components
{
    public interface IProcessRunner
    {
        Task<ProcessResult> RunProcessAsync(ProcessStartInfo startInfo, CancellationToken cancellationToken);
    }
}