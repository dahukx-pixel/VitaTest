using System.Windows;
using System.Windows.Controls;
using VitaTest.Domain.Models;

namespace VitaTest.Modules.Orders.Selectors
{
    public class OrderDialogTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PaymentTemplate { get; set; }
        public DataTemplate NewOrderTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Order order)
            {
                return PaymentTemplate;
            }
            else
            {
                return NewOrderTemplate;
            }
        }
    }
}
