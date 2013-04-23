using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Web.UI.Fields.Definitions;

namespace SitefinityWebApp.Widgets.FieldControls
{
    /// <summary>
    /// A control definition for the simple image field
    /// </summary>
    public class UserSelectorDefinition : FieldControlDefinition, IUserSelectorDefinition
    {
        #region Constuctors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSelectorDefinition"/> class.
        /// </summary>
        public UserSelectorDefinition()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSelectorDefinition"/> class.
        /// </summary>
        /// <param name="element">The configuration element used to persist the control definition.</param>
        public UserSelectorDefinition(ConfigElement element)
            : base(element)
        {
        }
        #endregion

        #region IUserSelectorDefinition members
        public string DynamicModuleType
        {
            get
            {
                return this.ResolveProperty("DynamicModuleType", this.dynamicModuleType);
            }
            set
            {
                this.dynamicModuleType = value;
            }
        }
        #endregion

        #region Private members
        private string dynamicModuleType;
        #endregion

    }
}