using System;
using System.IO;
using System.Security;
using System.Text;
using Org.BouncyCastle.Asn1.BC;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;

namespace GoLava.ApplePie.Security
{
    /// <summary>
    /// A certificate request factory.
    /// </summary>
    public class CertificateRequestFactory
    {
        private readonly SecureRandomProvider _secureRandomProvider;
        private readonly SecureStringConverter _secureStringConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GoLava.ApplePie.Security.CertificateRequestFactory"/> class.
        /// </summary>
        public CertificateRequestFactory()
        {
            _secureRandomProvider = new SecureRandomProvider();
            _secureStringConverter = new SecureStringConverter();
        }

        /// <summary>
        /// Creates a certificate request with private key.
        /// </summary>
        public CertificateRequestWithPrivateKey CreateCertificateRequestWithPrivateKey(SecureString securePassword)
        {
            var rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(
                _secureRandomProvider.GetSecureRandom(), 2048));
            

            var asymmetricCipherKeyPair = rsaKeyPairGenerator.GenerateKeyPair();
            var asn1SignatureFactory = new Asn1SignatureFactory(
                    "SHA1WITHRSA",
                    asymmetricCipherKeyPair.Private);
            var pkcs10CertificationRequest = new Pkcs10CertificationRequest(
                asn1SignatureFactory,
                new X509Name("CN=PEM"),
                asymmetricCipherKeyPair.Public,
                null,
                asymmetricCipherKeyPair.Private);
            var encryptedPrivateKey = this.EncryptPrivateKey(asymmetricCipherKeyPair.Private, securePassword);
            return new CertificateRequestWithPrivateKey
            {
                CertificateRequest = EncodePem(pkcs10CertificationRequest),
                PrivateKey = EncodePem(encryptedPrivateKey)
            };
        }

        private object EncryptPrivateKey(AsymmetricKeyParameter privateKey, SecureString securePassword, int iterationCount = 1000000)
        {
            var plainPassword = _secureStringConverter.ConvertSecureStringToPlainCharArray(securePassword);
            try
            {
                var pkcs8Generator = new Pkcs8Generator(privateKey, BCObjectIdentifiers.bc_pbe_sha256_pkcs12_aes256_cbc.Id)
                {
                    IterationCount = iterationCount,
                    Password = plainPassword,
                    SecureRandom = _secureRandomProvider.GetSecureRandom()
                };
                return pkcs8Generator.Generate();
            }
            finally
            {
                Array.Clear(plainPassword, 0, plainPassword.Length);
            }
        }

        private static string EncodePem(object obj)
        {
            var sb = new StringBuilder();

            using (var stringWriter = new StringWriter(sb))
            {
                var pemWriter = new PemWriter(stringWriter);
                pemWriter.WriteObject(obj);
            }

            return sb.ToString();
        }
    }
}
