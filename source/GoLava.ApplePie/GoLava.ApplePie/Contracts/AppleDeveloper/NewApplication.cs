using GoLava.ApplePie.Contracts.Attributes;

namespace GoLava.ApplePie.Contracts.AppleDeveloper
{
    /// <summary>
    /// Contract used to add a new application.
    /// </summary>
    [CsrfClass(CsrfClass.Application)]
    public class NewApplication : ApplicationFeatures
    {
        private bool _dataProtectionPermission;

        /// <summary>
        /// Gets or sets the name of the new application.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the new application.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the type of the new application.
        /// </summary>
        public ApplicationType Type { get; set; } = ApplicationType.Explicit;

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:NewApplication"/> has a data protection permission.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the data protection permission level of the new application.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the prefix of the application.
        /// </summary>
        internal string Prefix { get; set; }
    }
}
