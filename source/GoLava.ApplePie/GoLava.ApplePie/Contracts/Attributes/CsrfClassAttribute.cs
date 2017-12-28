using System;

namespace GoLava.ApplePie.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CsrfClassAttribute : Attribute
    {
        public CsrfClassAttribute(CsrfClass csrfClass)
        {
            this.CsrfClass = csrfClass;
        }

        public CsrfClass CsrfClass { get; }
    }
}