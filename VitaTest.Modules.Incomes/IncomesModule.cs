using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;
using VitaTest.AppCore;
using VitaTest.Infrastructure;
using VitaTest.Infrastructure.Interfaces;
using VitaTest.Modules.Incomes.Views;

namespace VitaTest.Modules.Incomes
{
    public class IncomesModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public IncomesModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(IncomesView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}