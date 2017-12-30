using System;
using System.Collections.Generic;
using System.Net;

namespace GoLava.ApplePie.Transfer.Cookies
{
    public class DomainCookies
    {
        public string Domain { get; set; }

        public List<Cookie> Cookies { get; set; }
    }
}