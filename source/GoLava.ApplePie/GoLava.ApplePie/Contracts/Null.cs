using GoLava.ApplePie.Contracts.Attributes;

namespace GoLava.ApplePie.Contracts
{
    public class Null
    {
        public Null(CsrfClass csrfClass)
        {
            this.CsrfClass = csrfClass;
        }

        public CsrfClass CsrfClass { get; }
    }
}
