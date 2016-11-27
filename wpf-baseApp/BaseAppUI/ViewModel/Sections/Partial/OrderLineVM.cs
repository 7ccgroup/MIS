using BaseAppData.Entity;
using BaseAppUI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BaseAppUI.ViewModel.Sections.Partial
{
   public class OrderLineVM:NotifyProperty
    {
      public POS_OrderDetails _line;

      public POS_ItemMaster _item;
      public OrderVM _parent;
       public OrderLineVM(POS_ItemMaster item,OrderVM parent, POS_OrderDetails line=null)
       {
           _parent = parent;
           _item = item;
            

           if (line == null){
               _line = new POS_OrderDetails();
               _line.fLineItemPrice = item.vItemPrice;
               _line.POS_ItemMasterId = _item.Id;


               Quantity++;
             

           }
             
           else _line = line;
       }

      
       public int Quantity
       {
           get
           {
               return _line.fOrderQty;
           }
           set
           {
               _line.fOrderQty = value;


                //_line.fLineSubTotal =(decimal) (_line.fLineItemPrice * _line.fOrderQty)+_line.fLineItemAddonTotal; //5/31/16.....saa
                _line.fLineSubTotal = (decimal)(_line.fLineItemPrice + _line.fLineItemAddonTotal) * _line.fOrderQty; // 6/1 .. Amjad Fix

                OnPropertyChanged("Quantity");

               OnPropertyChanged("Total");

               _parent.RefreshValues();

           }
       }

        public decimal AddonTotal
        {
            get
            {
                return _line.fLineItemAddonTotal;
            }
            set
            {
                _line.fLineItemAddonTotal = value;

                OnPropertyChanged("AddonTotal");

                OnPropertyChanged("Total");


            }
        }
        public string ItemName
       {
           get
           {
               return _item.vItemDesc1;
           }
       }
        public string ItemAddon
        {
            get
            {
                return _line.vLineitemaddon;
            }
        }
        public decimal Price {

           get { return _line.fLineItemPrice; }
       }

       public decimal Total
       {
           get { return _line.fLineSubTotal; }
       }


       private bool _quantityChanged;

       public bool QuantityChanged
       {
           get { return _quantityChanged; }
           set { _quantityChanged = value;
           OnPropertyChanged("QuantityChanged");
          
           }
       }

       public bool ItemCreated { get; set; }

       public SolidColorBrush Brush { get; set; }
     
       public string Comments
       {
           get { return _line.vComments; }
           set { _line.vComments = value;
           OnPropertyChanged("Comments");
           }
       }

      
     
    }
}
