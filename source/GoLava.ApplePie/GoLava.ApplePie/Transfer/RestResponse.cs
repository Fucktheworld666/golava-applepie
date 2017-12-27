using System.Net;

namespace GoLava.ApplePie.Transfer
{
    /// <summary>
    /// Response returned by <see cref="T:RestClient"/>.
    /// </summary>
    public class RestResponse<TContent>
    {
        /// <summary>
        /// Gets a value indicating whether rest call was successful.
        /// </summary>
        /// <value><c>true</c> if it was successful; otherwise, <c>false</c>.</value>
        public bool IsSuccess 
        {
            get { return (int)this.StatusCode >= 200 && (int)this.StatusCode <= 299; }   
        }

        /// <summary>
        /// Gets or sets the <see cref="T:HttpStatusCode"/> of the rest response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:RestHeaders"/> of the rest response.
        /// </summary>
        public RestHeaders Headers { get; set; }

        /// <summary>
        /// Gets or sets the parsed content of the rest response.
        /// </summary>
        public TContent Content { get; set; }

        /// <summary>
        /// Gets or sets the content of the raw (unparsed) content of the rest response.
        /// </summary>
        public string RawContent { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="RestContentType"/> of the rest response.
        /// </summary>
        public RestContentType ContentType { get; set; }
    }
}