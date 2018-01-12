using System;
namespace GoLava.ApplePie.Transfer.Content
{
    public class RawBinaryContent : RawContent
    {
        public RawBinaryContent(byte[] rawContent)
        {
            this.Value = rawContent;
        }

        public byte[] Value { get; }

        public override string ToString()
        {
            throw new NotSupportedException();
        }
    }
}