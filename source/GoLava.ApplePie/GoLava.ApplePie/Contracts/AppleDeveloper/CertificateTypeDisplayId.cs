using System.Runtime.Serialization;
using System.Text;
using GoLava.ApplePie.Extensions;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    public enum CertificateTypeDisplayId
    {
        [EnumMember(Value = "5QPB9NHCEI")]
        Development,
        [EnumMember(Value = "R58UK2EWSO")]
        Production,
        [EnumMember(Value = "9RQEK7MSXA")]
        InHouse,
        [EnumMember(Value = "LA30L5BJEU")]
        Certificate,
        [EnumMember(Value = "BKLRAVXMGM")]
        DevelopmentPush,
        [EnumMember(Value = "3BQKVH9I2X")]
        ProductionPush,
        [EnumMember(Value = "Y3B2F3TYSI")]
        Passbook,
        [EnumMember(Value = "3T2ZP62QW8")]
        WebsitePush1,
        [EnumMember(Value = "E5D663CMZW")]
        WebsitePush2,
        [EnumMember(Value = "4APLUP237T")]
        ApplePay,

        [EnumMember(Value = "749Y1QAGU7")]
        MacDevelopment,
        [EnumMember(Value = "HXZEUKP0FP")]
        MacAppDistribution,
        [EnumMember(Value = "2PQI8IDXNH")]
        MacInstallerDistribution,
        [EnumMember(Value = "OYVN2GW35E")]
        DeveloperIDInstaller,
        [EnumMember(Value = "W0EURJRMC5")]
        DeveloperIDApplication1,
        [EnumMember(Value = "CDZ7EMXIZ1")]
        MacProductionPush,
        [EnumMember(Value = "HQ4KP3I34R")]
        MacDevelopmentPush,
        [EnumMember(Value = "DIVN2GW3XT")]
        DeveloperIDApplication2
    }

    public static class CertificateTypeDisplayIds
    {
        public static readonly CertificateTypeDisplayId[] IosCertificateTypeDisplayIds = {
            CertificateTypeDisplayId.Development, CertificateTypeDisplayId.Production,
            CertificateTypeDisplayId.InHouse, CertificateTypeDisplayId.Certificate,
            CertificateTypeDisplayId.DevelopmentPush, CertificateTypeDisplayId.ProductionPush,
            CertificateTypeDisplayId.Passbook, CertificateTypeDisplayId.WebsitePush1,
            CertificateTypeDisplayId.WebsitePush2, CertificateTypeDisplayId.ApplePay
        };

        public static readonly CertificateTypeDisplayId[] MacCertificateTypeDisplayIds = {
            CertificateTypeDisplayId.MacDevelopment, CertificateTypeDisplayId.MacAppDistribution,
            CertificateTypeDisplayId.MacInstallerDistribution, CertificateTypeDisplayId.DeveloperIDInstaller,
            CertificateTypeDisplayId.DeveloperIDApplication1, CertificateTypeDisplayId.MacProductionPush,
            CertificateTypeDisplayId.MacDevelopmentPush, CertificateTypeDisplayId.DeveloperIDApplication2
        };

        public static readonly CertificateTypeDisplayId[] SafariCertificateTypeDisplayIds = {
            
        };

        public static CertificateTypeDisplayId[] GetPlatformIds(Platform platform)
        {
            switch (platform)
            {
                case Platform.Ios:
                    return CertificateTypeDisplayIds.IosCertificateTypeDisplayIds;
                case Platform.Mac:
                    return CertificateTypeDisplayIds.MacCertificateTypeDisplayIds;
                case Platform.Safari:
                    return CertificateTypeDisplayIds.SafariCertificateTypeDisplayIds;
                default:
                    return null;
            }
        }

        public static string ToStringValue(this CertificateTypeDisplayId[] ids)
        {
            var sb = new StringBuilder();
            if (ids != null)
            {
                foreach (var id in ids) 
                {
                    if (sb.Length > 0)
                        sb.Append(",");
                    sb.Append(id.ToStringValue());
                }
            }
            return sb.ToString();
        }
    }
}