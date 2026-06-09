using MaterialDesignThemes.Wpf;
using System.Windows;

namespace VitaTest.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ISnackbarMessageQueue messageQueue)
        {
            InitializeComponent();

            NotifySnackBar.MessageQueue = messageQueue as SnackbarMessageQueue;
        }
    }
}
