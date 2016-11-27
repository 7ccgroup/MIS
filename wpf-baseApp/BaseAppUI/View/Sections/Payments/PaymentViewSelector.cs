using BaseAppUI.ViewModel.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BaseAppUI.View.Sections.Payments
{
    public class PaymentViewSelector : DataTemplateSelector
    {
        public DataTemplate CardPaymentView { get; set; }
        public DataTemplate CashPaymentView { get; set; }
        public DataTemplate OtherPaymentView { get; set; }
        public DataTemplate NoTaxView { get; set; }
        public DataTemplate DiscountView { get; set; }
        public DataTemplate CouponView { get; set; }

        public DataTemplate SplitPaymentView { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is CardPayVM)
                return CardPaymentView;
            else if (item is CashPayVM)
                return CashPaymentView;
            else if (item is SplitPaymentVM)
                return SplitPaymentView;
          
               

            return base.SelectTemplate(item, container);
        }
    }
}
