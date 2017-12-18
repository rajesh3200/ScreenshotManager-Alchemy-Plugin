using Alchemy4Tridion.Plugins.GUI.Configuration;

namespace ScreenShotManager.Configuration
{
    public class ScreenShotManagerResourceGroup : ResourceGroup
    {
        public ScreenShotManagerResourceGroup() : base("CommandFiles")
        {
            AddFile("ScreenShotManager.js");
            this.AddWebApiProxy();
            this.Dependencies.AddLibraryJQuery();
        }
    }
}