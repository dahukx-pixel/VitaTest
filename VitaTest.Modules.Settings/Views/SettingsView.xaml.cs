using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VitaTest.Modules.Settings.ViewModels;

namespace VitaTest.Modules.Settings.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
            {
                vm.ProcessSave(PasswordBox.Password).Await();
            }
        }
    }
}
