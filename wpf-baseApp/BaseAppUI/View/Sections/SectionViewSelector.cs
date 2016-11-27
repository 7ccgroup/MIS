using BaseAppUI.ViewModel.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BaseAppUI.View.Sections
{
    public class SectionViewSelector : DataTemplateSelector
    {
        public DataTemplate DashboardView { get; set; }
        public DataTemplate OrdersView { get; set; }
        public DataTemplate RegisterView { get; set; }
        public DataTemplate PaymentView { get; set; }
        public DataTemplate ReportView { get; set; } //Syed 3/31 { 600 - 101

        public DataTemplate ItemsView { get; set; } //Syed 4/3 { 300 -101

        public DataTemplate CustomersView { get; set; } //Syed 4/3 { 300 -101

        public DataTemplate LoginView { get; set; } //Syed 4/3 { 300 -101
        public DataTemplate ReportsMainView { get; set; } //Syed 3/31 { 600 - 101
        public DataTemplate SettingsView { get; set; } //Syed 3/31 { 600 - 101
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DashBoard)
                return DashboardView;
            else if (item is Register)
                return RegisterView;
            else if (item is OrdersHistory)
                return OrdersView;
            else if (item is Payment)
                return PaymentView;
            else if (item is Report)
                return ReportView; ///this is Datagrid style reports.

            else if (item is ReportsMain)

                return ReportsMainView;
            else if (item is Items)
                return ItemsView;
            else if (item is Customers)
                return CustomersView;
            else if (item is Login)
                return LoginView;
            else if (item is PosSettings)
                return SettingsView;
            return base.SelectTemplate(item, container);
        }
    }
}
