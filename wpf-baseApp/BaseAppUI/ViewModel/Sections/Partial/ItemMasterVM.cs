using BaseAppData.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BaseAppUI.ViewModel.Sections.Partial
{
   public class ItemMasterVM:NotifyProperty
    {
     public  POS_ItemMaster _item;
       public ItemMasterVM(POS_ItemMaster item)
       {
           _item = item;

       }

       public string Title
       {
           get
           {
               return _item.vItemDesc1;
           }
       }

       public decimal Price
       {
           get { 
               return _item.vItemPrice;
           }
       }

       public decimal? RegularPrice
       {
           get
           {
               if (_item.vItemMinPrice == _item.vItemPrice)
                   return null;
               else 

               return _item.vItemMinPrice;
           }
       }


       public SolidColorBrush _borderBrush;
       public SolidColorBrush BorderBrush
       {
           get
           {
               return _borderBrush;

           }
           set
           {
                   _borderBrush = value;
                   OnPropertyChanged("BorderBrush");
           }

       }

       public string Itemmodifier
       {
           get
           {
               if (string.IsNullOrEmpty(_item.vItemmodifier))
               {
                 //  _item.vItemmodifier = "enabled";
                   _item.vItemmodifier = "disabled";

               }
               return _item.vItemmodifier.ToLower();
           }
           set
           {
               _item.vItemmodifier = value;
           }
       }

      
    }
}
