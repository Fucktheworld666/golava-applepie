using GoLava.ApplePie.Contracts.Attributes;

namespace GoLava.ApplePie.Contracts
{
    /// <summary>
    /// A contract that represents null but has a <see cref="CsrfClass"/> to get
    /// correct csrf values for the request it is used in.
    /// </summary>
    public class Null
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Null"/> class.
        /// </summary>
        /// <param name="csrfClass">The csrf class to be used.</param>
        public Null(CsrfClass csrfClass)
        {
            this.CsrfClass = csrfClass;
        }

        /// <summary>
        /// Gets the csrf class.
        /// </summary>
        public CsrfClass CsrfClass { get; }
    }
}