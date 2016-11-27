using BaseAppUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseAppUI.ViewModel.Sections.Partial
{
   public class OrderTypeVM:NotifyProperty
    {
       public OrdersHistory _parent;
       string _name;
      public string _ordertype;
       bool _isSelected;

      
       public OrderTypeVM(string name, string ordertype, OrdersHistory parent)
       {

           _name = name;
           _ordertype = ordertype;
           _parent = parent;

           SetIcon(this);


       }

       private static void SetIcon(OrderTypeVM vm)
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
