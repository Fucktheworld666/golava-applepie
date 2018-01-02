using GoLava.ApplePie.Transfer.Cookies;

namespace GoLava.ApplePie.Transfer
{
    /// <summary>
    /// A context to be shared between multiple rest calls to keep track of
    /// things like session, cookies and the like.
    /// </summary>
    public class RestClientContext
    {
        private CookieJar _cookieJar;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RestClientContext"/> class.
        /// </summary>
        public RestClientContext()
        {
            this.CookieJar = new CookieJar();
        }

        /// <summary>
        /// Gets or sets the <see cref="T:CookieJar"/> associated with this <see cref="T:RestClientContext"/>.
        /// </summary>
        public CookieJar CookieJar
        {
            get { return _cookieJar; }
            set { _cookieJar = value ?? new CookieJar(); }
        }
    }
}