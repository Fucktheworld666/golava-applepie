using System;
using GoLava.ApplePie.Clients.AppleDeveloper;
using GoLava.ApplePie.Clients.ApplePie;
using GoLava.ApplePie.Clients.ApplePie.CertificateStores;

namespace GoLava.ApplePie
{
    public class ApplePieOptions
    {
        internal Func<IServiceProvider, IAppleDeveloperUrlProvider> AppleDeveloperFactory;

        internal Func<IServiceProvider, ICertificateStore> CertificateStoreFactory;

        public ApplePieOptions()
        {
            AppleDeveloperFactory = new Func<IServiceProvider, IAppleDeveloperUrlProvider>(_ => new AppleDeveloperUrlProvider());
            CertificateStoreFactory = new Func<IServiceProvider, ICertificateStore>(_ => new FileCertificateStore());
        }

        public void UseAppleDeveloper(Func<IServiceProvider, IAppleDeveloperUrlProvider> factory)
        {
            AppleDeveloperFactory = factory;
        }

        public void UseCertificateStore(Func<IServiceProvider, ICertificateStore> factory)
        {
            CertificateStoreFactory = factory;
        }
    }
}
