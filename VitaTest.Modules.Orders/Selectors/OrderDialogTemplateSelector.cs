using System.Windows;
using System.Windows.Controls;
using VitaTest.Domain.Models;

namespace VitaTest.Modules.Orders.Selectors
{
    public class OrderDialogTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PaymentTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Order order)
            {
                if (order.BalanceToPay > 0)
                {
                    return PaymentTemplate;
                }
                else
                {
                    return DefaultTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
