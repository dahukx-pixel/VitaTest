using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;
using VitaTest.AppCore;
using VitaTest.Infrastructure;
using VitaTest.Infrastructure.Interfaces;
using VitaTest.Modules.Payments.Views;

namespace VitaTest.Modules.Payments
{
    public class PaymentsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public PaymentsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(PaymentsView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}