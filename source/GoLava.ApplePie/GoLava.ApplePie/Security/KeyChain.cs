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
        private static string[] knownKeychainPaths = { 
            "./Library/Keychains/login.keychain", 
            "./Library/Keychains/login.keychain-db" };

        private readonly IProcessRunner _processRunner;
        private readonly IEnvironmentDetector _environmentDetector;

        public Keychain(IProcessRunner processRunner, IEnvironmentDetector environmentDetector)
        {
            _processRunner = processRunner;
            _environmentDetector = environmentDetector;
        }

        public string GetDefaultKeychainFile()
        {
            var homeDirectory = _environmentDetector.HomeDirectory;
            return knownKeychainPaths
                .Select(p => Path.Combine(homeDirectory, p))
                .Last(File.Exists);
        }

        public Task<bool> ImportBinaryDataAsync(byte[] certificateData, string certificatePassword)
        {
            return this.ImportBinaryDataAsync(certificateData, certificatePassword, this.GetDefaultKeychainFile(), string.Empty);
        }

        public Task<bool> ImportFileAsync(string certificateFile, string certificatePassword)
        {
            return this.ImportFileAsync(certificateFile, certificatePassword, this.GetDefaultKeychainFile(), string.Empty);
        }

        public async Task<bool> ImportBinaryDataAsync(byte[] certificateData, string certificatePassword, string keychainFile, string keychainPassword)
        {
            await Configure.AwaitFalse();

            var tempPath = Path.GetTempPath();
            var tempFileName = Path.Combine(tempPath, $"{Guid.NewGuid().ToString("N")}.p12");
            try
            {
                using (var file = File.OpenWrite(tempFileName))
                    await file.WriteAsync(certificateData, 0, certificateData.Length);
                return await this.ImportFileAsync(tempFileName, certificatePassword, keychainFile, keychainPassword);
            }
            finally
            {
                File.Delete(tempFileName);
            }
        }

        public async Task<bool> ImportFileAsync(string certificateFile, string certificatePassword, string keychainFile, string keychainPassword)
        {
            if (string.IsNullOrEmpty(certificateFile))
                throw new ArgumentNullException(nameof(certificateFile));
            if (string.IsNullOrEmpty(keychainFile))
                throw new ArgumentNullException(nameof(keychainFile));

            await Configure.AwaitFalse();

            certificateFile = Path.GetFullPath(certificateFile);
            keychainFile = Path.GetFullPath(keychainFile);

            var importBuilder = new StringBuilder();
            importBuilder.Append($"import {certificateFile.ShellEscape()} -k {keychainFile.ShellEscape()}");
            importBuilder.Append($" -P {certificatePassword.ShellEscape()}");
            importBuilder.Append($" -T /usr/bin/codesign");
            importBuilder.Append($" -T /usr/bin/security");
            var importArgs = importBuilder.ToString();

            var importResult = await this.RunSecurityCommandAsync(importArgs);
            if (importResult.ExitCode != 0)
                return false;

            var helpResult = await this.RunSecurityCommandAsync("-h");
            if (importResult.ExitCode == 0 && importResult.Output.Any(o => o.Data.Contains("set-key-partition-list")))
            {
                var setKeyPartionListBuilder = new StringBuilder();
                setKeyPartionListBuilder.Append($"set-key-partition-list");
                setKeyPartionListBuilder.Append($" -S apple-tool:,apple:");
                setKeyPartionListBuilder.Append($" -k {keychainPassword.ShellEscape()}");
                setKeyPartionListBuilder.Append($" {keychainFile.ShellEscape()}");
                var setKeyPartionListArgs = setKeyPartionListBuilder.ToString();

                var setKeyPartionListResult = await this.RunSecurityCommandAsync(setKeyPartionListArgs);
                return setKeyPartionListResult.ExitCode == 0;
            }

            return true;
        }

        private Task<ProcessResult> RunSecurityCommandAsync(string arguments)
        {
            return _processRunner.RunProcessAsync(new ProcessStartInfo("/usr/bin/security", arguments)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            }, CancellationToken.None);
        }
    }
}
