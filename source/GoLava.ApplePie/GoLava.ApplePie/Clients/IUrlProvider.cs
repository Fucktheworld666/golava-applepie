namespace GoLava.ApplePie.Clients
{
    public interface IUrlProvider
    {
        string AuthTokenUrl { get; }

        string LogonUrl { get; }

        string SessionUrl { get; }
    }
}