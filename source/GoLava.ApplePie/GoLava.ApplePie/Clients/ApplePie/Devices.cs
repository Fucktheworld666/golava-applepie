using System;
namespace GoLava.ApplePie.Clients.ApplePie
{
    public class Devices
    {
        private readonly ApplePieClient _applePieClient;

        internal Devices(ApplePieClient applePieClient)
        {
            _applePieClient = applePieClient;
        }
    }
}
