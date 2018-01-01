using GoLava.ApplePie.Contracts.Attributes;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    [CsrfClass(CsrfClass.Application)]
    public class AddApplication : ApplicationFeatures
    {
        private bool _dataProtectionPermission;

        public string Name { get; set; }

        internal string Prefix { get; set; }

        public string Identifier { get; set; }

        public ApplicationType Type { get; set; }

        public bool DataProtectionPermission 
        { 
            get { return _dataProtectionPermission; }
            set 
            {
                if (_dataProtectionPermission != value)
                {
                    _dataProtectionPermission = value;
                    if (!_dataProtectionPermission)
                        this.DataProtectionPermissionLevel = null;
                    else if (this.DataProtectionPermissionLevel == null)
                        this.DataProtectionPermissionLevel = AppleDeveloper.DataProtectionPermissionLevel.Complete;
                }
            }
        }

        public override DataProtectionPermissionLevel? DataProtectionPermissionLevel
        {
            get { return base.DataProtectionPermissionLevel; }
            set
            {
                if (base.DataProtectionPermissionLevel != value)
                {
                    base.DataProtectionPermissionLevel = value;
                    _dataProtectionPermission = value != null;
                }
            }
        }
    }
}
