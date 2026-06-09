using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System.Windows.Input;
using VitaTest.AppCore;
using VitaTest.AppCore.Services.Interfaces;

namespace VitaTest.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager,
                                   INotificationService notificationService)
        {
            _regionManager = regionManager;
        }

        public ICommand NavigateToCommand => new DelegateCommand<string>(NavigateTo);

        private void NavigateTo(string pageName)
        {
            _regionManager.RequestNavigate(RegionNames.MainRegion, pageName);
        }
    }
}
