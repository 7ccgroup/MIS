using BaseAppUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.ViewModel.Payments
{
   public class PaymentSection:NotifyProperty
    {
       public string Name { get; set; }

       private bool _isSelected;

       public bool IsSelected
       {
           get { return _isSelected; }
           set { _isSelected = value;
           OnPropertyChanged("IsSelected");
           }
       }

       public PaymentSectionType PaymentSectionType { get; set; }
    }
}
