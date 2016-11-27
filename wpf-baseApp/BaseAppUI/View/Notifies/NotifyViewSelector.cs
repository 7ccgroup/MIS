using BaseAppUI.ViewModel.Notifies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BaseAppUI.View.Notifies
{
    public class NotifyViewSelector : DataTemplateSelector
    {
        public DataTemplate PaymentCompletedNotifyView { get; set; }
        public DataTemplate TransactionFailedNotifyView { get; set; }
        public DataTemplate TransactionSuccessNotifyView { get; set; }

        public DataTemplate LineItemModifierView { get; set; }
        public DataTemplate AuthorizingNotifyView { get; set; }
        public DataTemplate OrderTypeModifierView { get; set; }
        public DataTemplate CustomItemModifierView { get; set; }
        public DataTemplate TipSignModifierView { get; set; }
        public DataTemplate ThanksNotifyView { get; set; }
        public DataTemplate ItemEditView { get; set; }
        public DataTemplate CustomerEditView { get; set; }
        public DataTemplate SettingEditView { get; set; }
        public DataTemplate DiscountModifierView { get; set; }
        public DataTemplate AddTipModifierView { get; set; } // Tip Popup - Amjad
        public DataTemplate RestTableModifierView { get; set; } // Tip Popup - Amjad

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is PaymentCompletedNotify)
                return PaymentCompletedNotifyView;
            else if (item is TransactionFailedNotify)
                return TransactionFailedNotifyView;
            else if (item is TransactionSuccessNotify)
                return TransactionSuccessNotifyView;
            else if (item is LineItemModifier)
                return LineItemModifierView;
            else if (item is AuthorizingNotify)
                return AuthorizingNotifyView;
            else if (item is OrderTypeModifier)
                return OrderTypeModifierView;
            else if (item is CustomItemModifier)
                return CustomItemModifierView;
            else if (item is TipSignModifier)
                return TipSignModifierView;
            else if (item is ThanksNotify)
                return ThanksNotifyView;
            else if (item is ItemEdit)
                return ItemEditView;
            else if (item is CustomerEdit)
                return CustomerEditView;
            else if (item is SettingEdit)
                return SettingEditView;
            else if (item is DiscountModifier)
                return DiscountModifierView;
            else if (item is AddTipModifier) // Show Tip Popup - Amjad
                return AddTipModifierView;
            else if (item is RestTableModifier) // Show Table Modifier - SAA
                return RestTableModifierView;
            return base.SelectTemplate(item, container);
        }
    }
}
