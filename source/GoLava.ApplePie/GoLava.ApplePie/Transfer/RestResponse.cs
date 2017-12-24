using System.Net;

namespace GoLava.ApplePie.Transfer
{
    public class RestResponse<TContent>
    {
        public bool IsSuccess 
        {
            get { return (int)this.StatusCode >= 200 && (int)this.StatusCode <= 299; }   
        }

        public HttpStatusCode StatusCode { get; set; }

        public RestHeaders Headers { get; set; }

        public TContent Content { get; set; }

        public string RawContent { get; set; }

        public RestContentType ContentType { get; set; }
    }
}