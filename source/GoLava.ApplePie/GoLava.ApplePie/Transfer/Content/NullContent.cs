using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoLava.ApplePie.Transfer.Content
{
    /// <summary>
    /// A <see cref="T:HttpContent"/> that has no data and a content length of 0
    /// </summary>
    public class NullContent : HttpContent
    {
        private static Task Empty = Task.FromResult<object>(null);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:.NullContent"/> class.
        /// </summary>
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