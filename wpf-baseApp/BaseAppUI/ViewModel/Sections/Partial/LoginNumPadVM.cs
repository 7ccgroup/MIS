using BaseAppUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseAppUI.ViewModel.Sections.Partial
{
    public class LoginNumPadVM : NotifyProperty
    {
       
        string _num;
        public string _numType;
        bool _isSelected;


        public LoginNumPadVM(string num, string numtype) 
        {

            _num = num;
            _numType = numtype;
            //_parent = parent;

          //  SetIcon(this);


        }

   /*    private static void SetIcon(OrderTypeVM vm)
        {
            if (vm._ordertype == OrderTypes.None)
                vm.Icon = Application.Current.TryFindResource("icon_ordertype_all");
            else if (vm._ordertype == OrderTypes.DineIn)
                vm.Icon = Application.Current.TryFindResource("icon_dine_in");
            else if (vm._ordertype == OrderTypes.ToGo)
                vm.Icon = Application.Current.TryFindResource("icon_to_go");
            else if (vm._ordertype == OrderTypes.Catering)
                vm.Icon = Application.Current.TryFindResource("icon_catery");
            else if (vm._ordertype == OrderTypes.Delivery)
                vm.Icon = Application.Current.TryFindResource("icon_delivery");
            else if (vm._ordertype == OrderTypes.Online)
                vm.Icon = Application.Current.TryFindResource("icon_online");

        } */
        public string Num
        {
            get
            {
                return _num;
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
