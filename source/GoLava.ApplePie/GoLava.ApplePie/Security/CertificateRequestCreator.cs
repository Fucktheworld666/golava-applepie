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
    /// A certificate request creator.
    /// </summary>
    public class CertificateRequestCreator
    {
        private readonly SecureRandomProvider _secureRandomProvider;
        private readonly SecureStringConverter _secureStringConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GoLava.ApplePie.Security.CertificateRequestFactory"/> class.
        /// </summary>
        public CertificateRequestCreator()
        {
            _secureRandomProvider = new SecureRandomProvider();
            _secureStringConverter = new SecureStringConverter();
        }

        public CertificateRequestWithPrivateKey CreateCertificateRequestWithPrivateKey()
        {
            var rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(
                _secureRandomProvider.GetSecureRandom(), 2048));
            
            var asymmetricCipherKeyPair = rsaKeyPairGenerator.GenerateKeyPair();
            var asn1SignatureFactory = new Asn1SignatureFactory(
                    "SHA256WITHRSA",
                    asymmetricCipherKeyPair.Private);
            var pkcs10CertificationRequest = new Pkcs10CertificationRequest(
                asn1SignatureFactory,
                new X509Name("CN=PEM"),
                asymmetricCipherKeyPair.Public,
                null,
                asymmetricCipherKeyPair.Private);
            return new CertificateRequestWithPrivateKey
            {
                CertificateRequest = EncodePem(pkcs10CertificationRequest),
                PrivateKey = EncodePem(asymmetricCipherKeyPair.Private),
                PublicKey = EncodePem(asymmetricCipherKeyPair.Public)
            };
        }

        private object EncryptPrivateKey(AsymmetricKeyParameter privateKey, char[] password, int iterationCount = 1000000)
        {
            var pkcs8Generator = new Pkcs8Generator(privateKey, BCObjectIdentifiers.bc_pbe_sha256_pkcs12_aes256_cbc.Id)
            {
                IterationCount = iterationCount,
                Password = password,
                SecureRandom = _secureRandomProvider.GetSecureRandom()
            };
            return pkcs8Generator.Generate();
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
