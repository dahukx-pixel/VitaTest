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

        private async void PayButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button.Tag as Order;

            if (order == null) return;
            // Открываем диалог с передачей объекта
            await DialogHost.Show(order, "MainDialogHost");
        }
    }
}
