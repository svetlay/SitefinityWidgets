using System;
using System.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Web.UI.Fields.Config;

namespace SitefinityWebApp.Widgets.FieldControls
{
    /// <summary>
    /// A configuration element used to persist the properties of <see cref="UserSelectorDefinition"/>
    /// </summary>
    public class UserSelectorElement : FieldControlDefinitionElement, IUserSelectorDefinition
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSelectorElement"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public UserSelectorElement(ConfigElement parent)
            : base(parent)
        {
        }

        #endregion

        #region FieldControlDefinitionElement Members

        /// <summary>
        /// Gets an instance of the <see cref="UserSelectorDefinition"/> class.
        /// </summary>
        /// <returns></returns>
        public override DefinitionBase GetDefinition()
        {
            return new UserSelectorDefinition(this);
        }

        #endregion

        #region IFieldDefinition members

        public override Type DefaultFieldType
        {
            get
            {
                return typeof(UserSelector);
            }
        }

        #endregion

        #region IUserSelectorDefinition Members

        [ConfigurationProperty("DynamicModuleType")]
        public string DynamicModuleType
        {
            get
            {
                return (string)this["DynamicModuleType"];
            }
            set
            {
                this["DynamicModuleType"] = value;
            }
        }

        #endregion
    }
}