using System;
using System.Security;
using System.Threading.Tasks;
using GoLava.ApplePie.Clients;
using GoLava.ApplePie.Clients.AppleDeveloper;
using GoLava.ApplePie.Clients.ApplePie;
using GoLava.ApplePie.Components;
using GoLava.ApplePie.Contracts;
using GoLava.ApplePie.Security;
using GoLava.ApplePie.Security.CertificateStores;

namespace GoLava.ApplePie
{
    public class ApplePieClient
    {
        private readonly IAppleDeveloperClient _appleDeveloperClient;
        private Applications _applications;
        private ICertificates _certificates;
        private Devices _devices;
        private Teams _teams;

        public ApplePieClient(
            IAppleDeveloperClient appleDeveloperClient,
            ICertificates certificates)
        {
            _appleDeveloperClient = appleDeveloperClient;

            this.Certificates = certificates;
        }

        /// <summary>
        /// Gets access to the applications API.
        /// </summary>
        public Applications Applications
        {
            get { return _applications ?? (_applications = new Applications(this)); }
        }

        /// <summary>
        /// Gets access to the certificates API.
        /// </summary>
        public ICertificates Certificates { get; }

        /// <summary>
        /// Gets access to the devices API.
        /// </summary>
        public Devices Devices
        {
            get { return _devices ?? (_devices = new Devices(this)); }
        }

        /// <summary>
        /// Gets access to the teams API.
        /// </summary>
        public Teams Teams
        {
            get { return _teams ?? (_teams = new Teams(this)); }
        }

        /// <summary>
        /// Uses username and password to logon.
        /// </summary>
        /// <returns>Returns a <see cref="T:ClientContext"/> to be used with all following client calls.</returns>
        /// <param name="username">The username to use when logging on.</param>
        /// <param name="password">The password to use when logging on..</param>
        public Task<ClientContext> LogonWithCredentialsAsync(string username, string password)
        {
            return _appleDeveloperClient.LogonWithCredentialsAsync(username, password);
        }

        /// <summary>
        /// Uses username and password to logon.
        /// </summary>
        /// <returns>Returns a <see cref="T:ClientContext"/> to be used with all following client calls.</returns>
        /// <param name="username">The username to use when logging on.</param>
        /// <param name="password">The password to use when logging on..</param>
        public Task<ClientContext> LogonWithCredentialsAsync(string username, SecureString password)
        {
            return _appleDeveloperClient.LogonWithCredentialsAsync(username, password);
        }

        /// <summary>
        /// Acquires a the two-step authentication code to be send to the given trusted device.
        /// </summary>
        /// <returns>Returns a <see cref="T:ClientContext"/> to be used with all following client calls.</returns>
        /// <param name="context">The current context.</param>
        /// <param name="trustedDevice">Trusted device.</param>
        public Task<ClientContext> AcquireTwoStepCodeAsync(ClientContext context, TrustedDevice trustedDevice)
        {
            return _appleDeveloperClient.AcquireTwoStepCodeAsync(context, trustedDevice);
        }

        /// <summary>
        /// Uses a previous acquired two-step authentication code to log on.
        /// </summary>
        /// <returns>Returns a <see cref="T:ClientContext"/> to be used with all following client calls.</returns>
        /// <param name="context">The current context.</param>
        /// <param name="code">The code to be used to finalize the two-step authentication.</param>
        public Task<ClientContext> LogonWithTwoStepCodeAsync(ClientContext context, string code)
        {
            return _appleDeveloperClient.LogonWithTwoStepCodeAsync(context, code);
        }

        internal ICertificateStore CertificateStore { get; }

        internal IEnvironmentDetector EnvironmentDetector { get; }

        internal IKeychain Keychain { get; }
    }
}
