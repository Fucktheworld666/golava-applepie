using System;
namespace GoLava.ApplePie.Components
{
    public class EnvironmentDetector : IEnvironmentDetector
    {
        public bool IsMac => Environment.OSVersion.Platform == PlatformID.MacOSX;

        public bool IsWindows => Environment.OSVersion.Platform <= PlatformID.WinCE;

        public bool IsUnix => Environment.OSVersion.Platform == PlatformID.Unix;

        public string HomeDirectory =>
            this.IsWindows 
                ? Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%")
                    : Environment.GetEnvironmentVariable("HOME");
    }
}