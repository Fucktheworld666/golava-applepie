using GoLava.ApplePie.Transfer.Cookies;

namespace GoLava.ApplePie.Transfer
{
    public class RestClientContext
    {
        public RestClientContext()
        {
            this.CookieJar = new CookieJar();
        }

        public CookieJar CookieJar { get; set; }
    }
}