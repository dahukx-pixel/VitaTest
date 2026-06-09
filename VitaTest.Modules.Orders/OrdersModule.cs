using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation.Regions;
using VitaTest.AppCore;
using VitaTest.Infrastructure;
using VitaTest.Infrastructure.Interfaces;
using VitaTest.Modules.Orders.Views;

namespace VitaTest.Modules.Orders
{
    public class OrdersModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public OrdersModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(OrdersView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}