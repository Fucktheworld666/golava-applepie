using System;
using GoLava.ApplePie.Clients.AppleDeveloper;
using GoLava.ApplePie.Logging;
using GoLava.ApplePie.Security.CertificateStores;

namespace GoLava.ApplePie
{
    public class ApplePieOptions
    {
        internal Func<IServiceProvider, IAppleDeveloperUrlProvider> AppleDeveloperFactory;

        internal Func<IServiceProvider, ICertificateStore> CertificateStoreFactory;

        internal Func<IServiceProvider, ILogClient> LogClientFactory;

        public ApplePieOptions()
        {
            AppleDeveloperFactory = new Func<IServiceProvider, IAppleDeveloperUrlProvider>(_ => new AppleDeveloperUrlProvider());
            CertificateStoreFactory = new Func<IServiceProvider, ICertificateStore>(_ => new FileCertificateStore());
            LogClientFactory = new Func<IServiceProvider, ILogClient>(_ => new ConsoleLogClient());
        }

        public void UseAppleDeveloper(Func<IServiceProvider, IAppleDeveloperUrlProvider> factory)
        {
            AppleDeveloperFactory = factory;
        }

        public void UseCertificateStore(Func<IServiceProvider, ICertificateStore> factory)
        {
            CertificateStoreFactory = factory;
        }

        public void UseLogClient(Func<IServiceProvider, ILogClient> factory)
        {
            LogClientFactory = factory;
        }
    }
}
