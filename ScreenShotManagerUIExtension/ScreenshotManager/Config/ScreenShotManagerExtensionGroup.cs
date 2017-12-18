using Alchemy4Tridion.Plugins.GUI.Configuration;

namespace ScreenShotManager.Configuration
{
    class ScreenShotManagerExtensionGroup : ExtensionGroup
    {
        public ScreenShotManagerExtensionGroup()
        {
            AddExtension<ScreenShotManagerResourceGroup>("Tridion.Web.UI.Editors.CME.Views.Page");
        }
    }
}