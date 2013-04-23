using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Web.UI.Fields;
using Telerik.Sitefinity.Web.UI.Fields.Enums;
using Telerik.Web.UI;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Web.Configuration;
using Telerik.Sitefinity.Web.UI.Fields.Contracts;
using System.Web.UI.HtmlControls;

namespace SitefinityWebApp.Widgets.FieldControls
{
    /// <summary>
    /// A simple field control used to select a dynamic items.
    /// Use the path to this class when you add the field control in Sitefinity Module Builder
    /// SitefinityWebApp.Widgets.FieldControls.UserSelector
    /// </summary>
    [FieldDefinitionElement(typeof(UserSelectorElement))]
    public class UserSelector : FieldControl
    {
        #region Properties

        protected override WebControl TitleControl
        {
            get
            {
                return this.TitleLabel;
            }
        }

        protected override WebControl DescriptionControl
        {
            get
            {
                return this.DescriptionLabel;
            }
        }

        protected override WebControl ExampleControl
        {
            get
            {
                return this.ExampleLabel;
            }
        }

        protected override string LayoutTemplateName
        {
            get
            {
                return String.Empty;
            }
        }

        public override string LayoutTemplatePath
        {
            get
            {
                return UserSelector.layoutTemplate;
            }
            set
            {
                base.LayoutTemplatePath = value;
            }
        }

        /// <summary>
        /// Gets the title label.
        /// </summary>
        /// <value>The title label.</value>
        protected internal virtual Label TitleLabel
        {
            get
            {
                SitefinityLabel titleLabel = this.Container.GetControl<SitefinityLabel>("titleLabel", true);
                return titleLabel;
            }
        }

        /// <summary>
        /// Gets the description label.
        /// </summary>
        /// <value>The description label.</value>
        protected internal virtual Label DescriptionLabel
        {
            get
            {
                SitefinityLabel descriptionLabel = this.Container.GetControl<SitefinityLabel>("descriptionLabel", true);
                return descriptionLabel;
            }
        }

        /// <summary>
        /// Gets the example label.
        /// </summary>
        /// <value>The example label.</value>
        protected internal virtual Label ExampleLabel
        {
            get
            {
                SitefinityLabel exampleLabel = this.Container.GetControl<SitefinityLabel>("exampleLabel", true);
                return exampleLabel;
            }
        }

        /// <summary>
        /// Get a reference to the content selector
        /// </summary>
        protected virtual Telerik.Sitefinity.Web.UI.Backend.Security.Principals.UserSelector ItemsSelector
        {
            get
            {
                return this.Container.GetControl<Telerik.Sitefinity.Web.UI.Backend.Security.Principals.UserSelector>("itemsSelector", true);
            }
        }

        /// <summary>
        /// Get a reference to the selected items list
        /// </summary>
        protected virtual HtmlGenericControl SelectedItemsList
        {
            get
            {
                return this.Container.GetControl<HtmlGenericControl>("selectedItemsList", true);
            }
        }

        /// <summary>
        /// Get a reference to the selector wrapper
        /// </summary>
        protected virtual HtmlGenericControl SelectorWrapper
        {
            get
            {
                return this.Container.GetControl<HtmlGenericControl>("selectorWrapper", true);
            }
        }

        /// <summary>
        /// The LinkButton for "Done"
        /// </summary>
        protected virtual LinkButton DoneButton
        {
            get
            {
                return this.Container.GetControl<LinkButton>("doneButton", true);
            }
        }

        /// <summary>
        /// The LinkButton for "Cancel"
        /// </summary>
        protected virtual LinkButton CancelButton
        {
            get
            {
                return this.Container.GetControl<LinkButton>("cancelButton", true);
            }
        }

        /// <summary>
        /// The button area control
        /// </summary>
        protected virtual Control ButtonArea
        {
            get
            {
                return this.Container.GetControl<Control>("buttonAreaPanel", false);
            }
        }

        /// <summary>
        /// Get a reference to the link that opens the selector
        /// </summary>
        protected virtual HyperLink SelectButton
        {
            get
            {
                return this.Container.GetControl<HyperLink>("selectButton", true);
            }
        }

        /// <summary>
        /// Get or set the dynamic module type 
        /// </summary>
        public string DynamicModuleType
        {
            get
            {
                return this.dynamicModuleType;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.dynamicModuleType = value;
                }
            }
        }

        #endregion

        #region Overridden Methods

        protected override void InitializeControls(GenericContainer container)
        {
            this.TitleLabel.Text = this.Title;
            this.DescriptionLabel.Text = this.Description;
            this.ExampleLabel.Text = this.Example;
            
            this.ItemsSelector.ItemType = this.DynamicModuleType;
        }

        public override void Configure(IFieldDefinition definition)
        {
            base.Configure(definition);

            IUserSelectorDefinition fieldDefinition = definition as IUserSelectorDefinition;

            if (fieldDefinition != null)
            {
                if (!string.IsNullOrEmpty(fieldDefinition.DynamicModuleType))
                {
                    this.DynamicModuleType = fieldDefinition.DynamicModuleType;
                }
            }
        }

        #endregion

        #region IScriptControl Members

        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            List<ScriptReference> scripts = new List<ScriptReference>(base.GetScriptReferences());

            scripts.Add(new ScriptReference(UserSelector.scriptReference));

            scripts.Add(new ScriptReference("Telerik.Sitefinity.Resources.Scripts.jquery-ui-1.8.8.custom.min.js", "Telerik.Sitefinity.Resources"));
            scripts.Add(new ScriptReference("Telerik.Sitefinity.Resources.Scripts.Kendo.kendo.all.min.js",
                Config.Get<ControlsConfig>().ResourcesAssemblyInfo.Assembly.FullName));

            return scripts;
        }

        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var descriptors = new List<ScriptDescriptor>(base.GetScriptDescriptors());
            var lastDescriptor = (ScriptControlDescriptor)descriptors.Last();
            lastDescriptor.AddElementProperty("selectButton", this.SelectButton.ClientID);
            lastDescriptor.AddComponentProperty("itemsSelector", this.ItemsSelector.ClientID);
            lastDescriptor.AddElementProperty("selectorWrapper", this.SelectorWrapper.ClientID);
            lastDescriptor.AddElementProperty("selectedItemsList", this.SelectedItemsList.ClientID);
            lastDescriptor.AddElementProperty("doneButton", this.DoneButton.ClientID);
            lastDescriptor.AddElementProperty("cancelButton", this.CancelButton.ClientID);
            lastDescriptor.AddProperty("dynamicModulesDataServicePath", RouteHelper.ResolveUrl(UserSelector.DynamicModulesDataServicePath, UrlResolveOptions.Rooted));
            lastDescriptor.AddProperty("dynamicModuleType", this.DynamicModuleType);
            return descriptors;
        }

        #endregion

        #region Private Fields

        private const string DynamicModulesDataServicePath = "~/Sitefinity/Services/Security/Users.svc/";

        private static readonly string scriptReference = "~/Widgets/FieldControls/UserSelector.js";
        private static readonly string layoutTemplate = "~/Widgets/FieldControls/UserSelector.ascx";

        private string dynamicModuleType = "Telerik.Sitefinity.Security.Model.User";


        #endregion
    }
}