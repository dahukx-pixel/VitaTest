using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using VitaTest.Domain.Models;

namespace VitaTest.Modules.Orders.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        public OrdersView()
        {
            InitializeComponent();
        }

        private async void OpenHostDialogButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button.Tag as Order;

            await DialogHost.Show(order ?? new object(), "MainDialogHost");
        }
    }
}
