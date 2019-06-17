namespace GoLava.ApplePie.Components
{
    public interface IEnvironmentDetector
    {
        bool IsMac { get; }

        bool IsUnix { get; }

        bool IsWindows { get; }

        string HomeDirectory { get; }
    }
}