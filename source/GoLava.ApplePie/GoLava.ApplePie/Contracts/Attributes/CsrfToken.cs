namespace GoLava.ApplePie.Contracts.Attributes
{
    public class CsrfToken
    {
        public CsrfClass Class { get; set; }

        public string Value { get; set; }

        public string Timestamp { get; set; }
    }
}