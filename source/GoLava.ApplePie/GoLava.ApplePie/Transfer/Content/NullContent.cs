using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoLava.ApplePie.Transfer.Content
{
    public class NullContent : HttpContent
    {
        private static Task Empty = Task.FromResult<object>(null);

        public NullContent()
        {
            this.Headers.ContentLength = 0;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Empty;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return true;
        }
    }
}
