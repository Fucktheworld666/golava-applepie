using System.IO;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace GoLava.ApplePie.Security
{
    public class PrivateCertificateCreator
    {
        public PrivateCertificate CreatePrivateCertificate(X509Certificate2 certificate, string privateKey, char[] password)
        {
            var certParser = new X509CertificateParser();
            var cert = certParser.ReadCertificate(certificate.RawData);
            var certEntry = new X509CertificateEntry(cert);

            var privateKeyParameter = this.ReadPrivateKeyFromPemEncodedString(privateKey);

            var builder = new Pkcs12StoreBuilder();
            builder.SetUseDerEncoding(true);

            var store = builder.Build();
            store.SetKeyEntry(
                certificate.Subject, 
                new AsymmetricKeyEntry(privateKeyParameter), 
                new X509CertificateEntry[] { certEntry });

            var secureRandomProvider = new SecureRandomProvider();
            using (var stream = new MemoryStream())
            {
                store.Save(stream, password, secureRandomProvider.GetSecureRandom());

                var pfxBytes = stream.ToArray();
                return new PrivateCertificate
                {
                    Pkcs12 = Pkcs12Utilities.ConvertToDefiniteLength(pfxBytes)
                };
            }
        }

        private AsymmetricKeyParameter ReadPrivateKeyFromPemEncodedString(string privateKey)
        {
            using (var stringReader = new StringReader(privateKey))
            {
                var pemReader = new PemReader(stringReader);
                var pemObject = pemReader.ReadObject();
                return ((AsymmetricCipherKeyPair)pemObject).Private;
            }
        }
    }
}
