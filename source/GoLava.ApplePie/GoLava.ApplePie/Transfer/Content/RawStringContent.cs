namespace GoLava.ApplePie.Transfer.Content
{
    public class RawStringContent : RawContent
    {
        public RawStringContent(string rawContent)
        {
            this.Value = rawContent;
        }

        public string Value { get; }

        public override string ToString()
        {
            return this.Value;
        }
    }
}