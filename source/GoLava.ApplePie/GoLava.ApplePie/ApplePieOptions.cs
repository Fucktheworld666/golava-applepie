using System;
using GoLava.ApplePie.Clients.AppleDeveloper;

namespace GoLava.ApplePie
{
    public class ApplePieOptions
    {
        internal Func<IServiceProvider, IAppleDeveloperUrlProvider> AppleDeveloperFactory;

        public ApplePieOptions()
        {
            AppleDeveloperFactory = new Func<IServiceProvider, IAppleDeveloperUrlProvider>(_ => new AppleDeveloperUrlProvider());
        }

        public void UseAppleDeveloper(Func<IServiceProvider, IAppleDeveloperUrlProvider> factory)
        {
            AppleDeveloperFactory = factory;
        }
    }
}
