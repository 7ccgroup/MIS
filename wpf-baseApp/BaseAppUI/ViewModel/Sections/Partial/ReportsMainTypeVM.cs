using BaseAppUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseAppUI.ViewModel.Sections.Partial
{
    public class ReportsMainTypeVM : NotifyProperty
    {
        public ReportsMain _parent;
        string _name;
        public string _reporttype;
        bool _isSelected;


        public ReportsMainTypeVM(string name, string reporttype, ReportsMain parent)
        {

            _name = name;
            _reporttype = reporttype;
            _parent = parent;

            SetIcon(this);


        }

        private static void SetIcon(ReportsMainTypeVM vm)
        {
            if (vm._reporttype == ReportTypes.None)
                vm.Icon = Application.Current.TryFindResource("icon_reporttype_all");
            else if (vm._reporttype == ReportTypes.Sales)
                vm.Icon = Application.Current.TryFindResource("icon_sales");
            else if (vm._reporttype == ReportTypes.Customers)
                vm.Icon = Application.Current.TryFindResource("icon_customer");
            else if (vm._reporttype == ReportTypes.Items)
                vm.Icon = Application.Current.TryFindResource("icon_items");
            else if (vm._reporttype == ReportTypes.Pricing)
                vm.Icon = Application.Current.TryFindResource("icon_pricing");
            else if (vm._reporttype == ReportTypes.Inventory)
                vm.Icon = Application.Current.TryFindResource("icon_inventory");

        }
        public string Name
        {
            get
            {
                return _name;
            }

        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public object Icon { get; set; }


    }
}
