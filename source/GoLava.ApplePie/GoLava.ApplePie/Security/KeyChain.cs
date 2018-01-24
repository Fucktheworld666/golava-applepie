using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoLava.ApplePie.Components;
using GoLava.ApplePie.Extensions;
using GoLava.ApplePie.Threading;

namespace GoLava.ApplePie.Security
{
    public class Keychain : IKeychain
    {
        private readonly IProcessRunner _processRunner;

        public Keychain(IProcessRunner processRunner)
        {
            _processRunner = processRunner;
        }

        public async Task<bool> ImportAsync(string path, string keychainPath, string keychainPassword, string certificatePassword)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            if (string.IsNullOrEmpty(keychainPath))
                throw new ArgumentNullException(nameof(keychainPath));

            await Configure.AwaitFalse();

            path = Path.GetFullPath(path);
            keychainPath = Path.GetFullPath(keychainPath);

            var import = new StringBuilder();
            import.Append($"import {path.ShellEscape()} -k '{keychainPath.ShellEscape()}'");
            import.Append($" -P {certificatePassword.ShellEscape()}");
            import.Append($" -T /usr/bin/codesign");
            import.Append($" -T /usr/bin/security");

            var importResult = await this.RunSecurityCommandAsync(import.ToString());
            if (importResult.ExitCode != 0)
                return false;

            var helpResult = await this.RunSecurityCommandAsync("-h");
            if (importResult.ExitCode == 0 && importResult.Output.Any(o => o.Data.Contains("set-key-partition-list")))
            {
                var setKeyPartionList = new StringBuilder();
                setKeyPartionList.Append($"set-key-partition-list");
                setKeyPartionList.Append($" -S apple-tool:,apple:");
                setKeyPartionList.Append($" -k {keychainPassword.ShellEscape()}");
                setKeyPartionList.Append($" {keychainPath.ShellEscape()}");

                var setKeyPartionListResult = await this.RunSecurityCommandAsync(setKeyPartionList.ToString());
                return setKeyPartionListResult.ExitCode == 0;
            }

            return true;
        }

        private Task<ProcessResult> RunSecurityCommandAsync(string arguments)
        {
            return _processRunner.RunProcessAsync(new ProcessStartInfo("/usr/bin/security")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = arguments
            }, CancellationToken.None);
        }
    }
}
