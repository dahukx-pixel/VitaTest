using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;
using VitaTest.AppCore;
using VitaTest.Modules.Settings.Views;

namespace VitaTest.Modules.Settings
{
    public class SettingsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public SettingsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(SettingsView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}