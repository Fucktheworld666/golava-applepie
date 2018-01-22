using System.IO;
using System.Text;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace GoLava.ApplePie.Security
{
    public class CertificateRequestFactory
    {
        private static readonly SecureRandom SecureRandom = new SecureRandom();

        public CertificateRequestWithPrivateKey CreateCertificateRequest()
        {
            var rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(
                SecureRandom, 2048));

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

            return new CertificateRequestWithPrivateKey
            {
                CertificateRequest = this.EncodeToPem(pkcs10CertificationRequest),
                PrivateKey = this.EncodeToPem(asymmetricCipherKeyPair.Private)
            };
        }

        private string EncodeToPem(object obj)
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
