using System;
using GoLava.ApplePie.Clients.AppleDeveloper;
using GoLava.ApplePie.Logging;
using GoLava.ApplePie.Security.CertificateStores;

namespace GoLava.ApplePie
{
    /// <summary>
    /// Options to be used for ApplePie.
    /// </summary>
    public class ApplePieOptions
    {
        internal Func<IServiceProvider, IAppleDeveloperUrlProvider> AppleDeveloperFactory;

        internal Func<IServiceProvider, ICertificateStore> CertificateStoreFactory;

        internal Func<IServiceProvider, ILogClient> LogClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplePieOptions"/> class.
        /// </summary>
        public ApplePieOptions()
        {
            AppleDeveloperFactory = new Func<IServiceProvider, IAppleDeveloperUrlProvider>(_ => new AppleDeveloperUrlProvider());
            CertificateStoreFactory = new Func<IServiceProvider, ICertificateStore>(_ => new FileCertificateStore());
            LogClientFactory = new Func<IServiceProvider, ILogClient>(_ => new ConsoleLogClient());
        }

        /// <summary>
        /// Uses the apple developer URL provider factory provided.
        /// </summary>
        /// <param name="factory">The factory to be used.</param>
        public void UseAppleDeveloperUrlProvider(Func<IServiceProvider, IAppleDeveloperUrlProvider> factory)
        {
            AppleDeveloperFactory = factory
                ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Uses the certificate store factory provided.
        /// </summary>
        /// <param name="factory">The factory to be used.</param>
        public void UseCertificateStore(Func<IServiceProvider, ICertificateStore> factory)
        {
            CertificateStoreFactory = factory
                ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Uses the log client factory provided.
        /// </summary>
        /// <param name="factory">The factory to be used.</param>
        public void UseLogClient(Func<IServiceProvider, ILogClient> factory)
        {
            LogClientFactory = factory
                ?? throw new ArgumentNullException(nameof(factory));
        }
    }
}