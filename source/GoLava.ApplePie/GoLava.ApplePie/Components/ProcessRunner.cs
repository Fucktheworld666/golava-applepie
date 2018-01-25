using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GoLava.ApplePie.Components
{
    public class ProcessRunner : IProcessRunner
    {
        public Task<ProcessResult> RunProcessAsync(ProcessStartInfo startInfo, CancellationToken cancellationToken)
        {
            if (startInfo == null)
                throw new ArgumentNullException(nameof(startInfo));

            var tcs = new TaskCompletionSource<ProcessResult>();
            var result = new ProcessResult
            {
                Output = new List<ProcessOutput>(),
                StartTime = DateTime.Now,
            };

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            var process = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            var cancellationTokenRegistration = cancellationToken.Register(() =>
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                        result.IsCanceled = true;
                    }
                }
                catch (InvalidOperationException)
                {
                    // ignore
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            void onOutputDataReceived(object s, DataReceivedEventArgs e)
            {
                if (!string.IsNullOrEmpty(e.Data))
                    result.Output.Add(new ProcessOutput(e.Data, false));
            }
            void onErrorDataReceived(object s, DataReceivedEventArgs e)
            {
                if (!string.IsNullOrEmpty(e.Data))
                    result.Output.Add(new ProcessOutput(e.Data, true));
            }
            void onProcessExited(object s, EventArgs e)
            {
                process.WaitForExit(-1);

                process.OutputDataReceived -= onOutputDataReceived;
                process.ErrorDataReceived -= onErrorDataReceived;
                process.Exited -= onProcessExited;

                cancellationTokenRegistration.Dispose();

                result.ExitCode = process.ExitCode;
                result.ExitTime = process.ExitTime;

                tcs.TrySetResult(result);
            }

            process.OutputDataReceived += onOutputDataReceived;
            process.ErrorDataReceived += onErrorDataReceived;
            process.Exited += onProcessExited;

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;
        }
    }
}
