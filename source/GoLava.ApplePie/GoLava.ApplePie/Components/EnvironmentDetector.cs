using System;
namespace GoLava.ApplePie.Components
{
    public class EnvironmentDetector : IEnvironmentDetector
    {
        public bool IsMac => Environment.OSVersion.Platform == PlatformID.MacOSX;

        public bool IsWindows => Environment.OSVersion.Platform == PlatformID.Win32NT;

        public bool IsUnix => Environment.OSVersion.Platform == PlatformID.Unix;
    }
}
