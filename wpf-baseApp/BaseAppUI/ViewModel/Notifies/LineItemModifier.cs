using Newtonsoft.Json;
using BaseAppData.Entity;
using BaseAppUI.Configuration;
using BaseAppUI.Controls;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseAppUI.ViewModel.Notifies
{
    public enum LineItemModifierButton
    {
        Addition,
        Subtract,
        Delete,
        Done
    }
   public class LineItemModifier:NotifyBase
    {
       OrderLineVM _lineitem;
       POS_OrderDetails _line;
       Action _onDone;

       public LineItemModifier(OrderLineVM lineitem,Action onDone=null)
       {
           _lineitem = lineitem;
           _line = lineitem._line;
           IsPopup = true;
           EnableMask = true;
           _onDone = onDone;
           _quantity = _lineitem.Quantity;


           _size = _line.vLineItemsize;
           _spice = _line.vLineItemspice;
           _cooked = _line.vLineItemcooked;
           _customComments=_line.vCustomComments;
           

           if (!string.IsNullOrEmpty(_line.vLineitemaddon))
           {
               var addonpairs = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(_line.vLineitemaddon);

               foreach (var n in Addons)
                   if (addonpairs.ContainsKey(n.Key))
                   {
                       n.IsSelected = true;
                       n.Value = addonpairs[n.Key];
                   }


           }

           

         
           
       }

       public string Title
       {
           get { return _lineitem.ItemName; }
       }

       private int _quantity;

       public int Quantity
       {
           get { return _quantity; }
           set
           {
               _quantity = value;
               OnPropertyChanged("Quantity");
               OnPropertyChanged("Subtotal");
           }
       }

       public decimal Price
       {
           get { return _lineitem.Price; }
       }

       public decimal Subtotal
       {
           get { return (decimal)(Price * Quantity)+ (AddonTotal * Quantity); } //5/31/16 added addon total.
       }

      
       private DelegateCommand<LineItemModifierButton> _lineItemCommand;

       public DelegateCommand<LineItemModifierButton> LineItemCommand
       {
           get { return _lineItemCommand ?? (_lineItemCommand = new DelegateCommand<LineItemModifierButton>((e) => {

               switch (e)
               {

                   case LineItemModifierButton.Addition:
                       Quantity++;
                       break;
                   case LineItemModifierButton.Subtract:
                       if (Quantity < 2) return;
                       else Quantity--;
                       break;
                   case LineItemModifierButton.Delete:

                       var ordervm = _lineitem._parent;
                       ordervm.OrderLines.Remove(_lineitem);
                       if(_lineitem._line.Id!=0)
                       GConfig.DAL.StateChange(_lineitem._line, System.Data.Entity.EntityState.Deleted);
                       ordervm.RefreshValues();
                       MainVM.Main.Register.RemoveSelection(_lineitem._item.Id);
                       this.CloseCommand.Execute(null);


                       break;
                   case LineItemModifierButton.Done:
                       _lineitem.AddonTotal = AddonTotal;
                       _lineitem.Quantity = Quantity;
                       _lineitem.Comments = applyDisplayformat();
                       _line.vLineItemsize = Size;
                       _line.vLineItemspice = Spice;
                       _line.vLineItemcooked = Cooked;
                       _line.vCustomComments=CustomComments;
                    

                       Dictionary<string, decimal> pairs = new Dictionary<string, decimal>();
                       foreach (var n in Addons.Where(m=>m.IsSelected))
                           pairs[n.Key] = (decimal)n.Value;
                       _line.vLineitemaddon = JsonConvert.SerializeObject(pairs);

                      

                       this.CloseCommand.Execute(null);
                       if (_onDone != null)
                           _onDone.Invoke();
                       break;


                   default: break;
               }
           
           })); }
       }


       private string _size;
      public string Size
       {
           get
           {
               return _size;
           }
           set
           {
               _size = value;
           



           }
       }

      private string _spice;
       public string Spice
       {
           get
           {
               return _spice;
           }
           set
           {
               _spice = value;
              


           }
       }

       private string _cooked;
       public string Cooked
       {
           get
           {
               return _cooked;
           }
           set
           {
               _cooked= value;
            
              
           }
       }

       


       private IList<ToggleItem> _sizes;
       private IList<ToggleItem> _spices;
       private IList<ToggleItem> _adons;
       private IList<ToggleItem> _cookeds;


       public IList<ToggleItem> Sizes
       {
           get {
               
               return _sizes ?? (_sizes = new List<ToggleItem> {
                   new ToggleItem { Text = "SMALL",Value="Small" },
                   new ToggleItem { Text = "MEDIUM",Value="Medium" },
                   new ToggleItem { Text = "LARGE",Value="Large" } }); }
       }

     

       public IList<ToggleItem> Spices
       {
           get
           {

               return _spices ?? (_spices = new List<ToggleItem> { 
                   new ToggleItem { Text = "NO SPICE",Value="No Spice" },
                   new ToggleItem { Text = "MILD", Value = "Mild" } ,
                   new ToggleItem { Text = "REGULAR", Value = "Regular" },
                   new ToggleItem { Text = "SPICY", Value = "Spicy" } ,
                   new ToggleItem { Text = "VERY SPICY", Value = "Very Spicy" } 


               
               });
           }
       }


     

       public IList<ToggleItem> Cookeds
       {
           get
           {

               return _cookeds ?? (_cookeds = new List<ToggleItem> { 
                   new ToggleItem { Text = "RARE",Value="Rare" },
                   new ToggleItem { Text = "MEDIUM RARE", Value ="Medium Rare" },
                   new ToggleItem { Text = "REGULAR", Value ="Regular" },
                   new ToggleItem { Text = "WELL DONE", Value ="Well Done" }
                 


               
               });
           }
       }

       public IList<ToggleItem> Addons
       {
           get
           {
                char[] _addonDelimiter = new char[1];
                _addonDelimiter[0] = ';';
                if (BaseAppUI.Properties.Settings.Default.AddonCount > 0)
                {
                    return _adons ?? (_adons = new List<ToggleItem> {

                   new ToggleItem(ChangeAddontotal) { Text = BaseAppUI.Properties.Settings.Default.Addon201.Split(_addonDelimiter)[0],
                       Value =Convert.ToDecimal (BaseAppUI.Properties.Settings.Default.Addon201.Split(_addonDelimiter)[1]),
                       Key = BaseAppUI.Properties.Settings.Default.Addon201.Split(_addonDelimiter)[2] },
                    new ToggleItem(ChangeAddontotal) { Text = BaseAppUI.Properties.Settings.Default.Addon202.Split(_addonDelimiter)[0],
                       Value =Convert.ToDecimal (BaseAppUI.Properties.Settings.Default.Addon202.Split(_addonDelimiter)[1]),
                       Key = BaseAppUI.Properties.Settings.Default.Addon202.Split(_addonDelimiter)[2] },
                     new ToggleItem(ChangeAddontotal) { Text = BaseAppUI.Properties.Settings.Default.Addon203.Split(_addonDelimiter)[0],
                       Value =Convert.ToDecimal (BaseAppUI.Properties.Settings.Default.Addon203.Split(_addonDelimiter)[1]),
                       Key = BaseAppUI.Properties.Settings.Default.Addon203.Split(_addonDelimiter)[2] },
                      new ToggleItem(ChangeAddontotal) { Text = BaseAppUI.Properties.Settings.Default.Addon204.Split(_addonDelimiter)[0],
                       Value =Convert.ToDecimal (BaseAppUI.Properties.Settings.Default.Addon204.Split(_addonDelimiter)[1]),
                       Key = BaseAppUI.Properties.Settings.Default.Addon204.Split(_addonDelimiter)[2] },
                      new ToggleItem(ChangeAddontotal) { Text = BaseAppUI.Properties.Settings.Default.Addon205.Split(_addonDelimiter)[0],
                       Value =Convert.ToDecimal (BaseAppUI.Properties.Settings.Default.Addon205.Split(_addonDelimiter)[1]),
                       Key = BaseAppUI.Properties.Settings.Default.Addon205.Split(_addonDelimiter)[2] }
                   //new ToggleItem(ChangeAddontotal) { Text = "ADD EGG",Value=.75m,Key="egg" },
                   //new ToggleItem(ChangeAddontotal) { Text = "ADD SCOOP", Value = 2.5m ,Key="scoop"},
                   // new ToggleItem(ChangeAddontotal) { Text = "ADD flavor",Value=.5m,Key="flavor" }
                   //new ToggleItem(ChangeAddontotal) { Text = "ADD YOGHURT", Value = 1.5m ,Key="yoghurt"}


            });
                }
                else
                {
                    return _adons ?? (_adons = new List<ToggleItem> {

                   
                   new ToggleItem(ChangeAddontotal) { Text = "ADD RICE",Value=.75m,Key="rice" },
                     new ToggleItem(ChangeAddontotal) { Text = "ADD PLAIN",Value=1.75m,Key="plain" },
                   new ToggleItem(ChangeAddontotal) { Text = "ADD YOGHURT", Value = 2.5m ,Key="yoghurt"}
                    //new ToggleItem(ChangeAddontotal) { Text = "ADD flavor",Value=.5m,Key="flavor" }
                   //new ToggleItem(ChangeAddontotal) { Text = "ADD YOGHURT", Value = 1.5m ,Key="yoghurt"}


            });
                }
           }
       }

       private decimal _addonTotal;

       public decimal AddonTotal
       {
           get { return _addonTotal; }
           set { _addonTotal = value;

           OnPropertyChanged("AddonTotal");
                OnPropertyChanged("Subtotal"); //5/31/2016 for crepe
            }
       }

      
       private void ChangeAddontotal()  //This add the value of all selected Addon.
       {
           var sum = this.Addons.Where(n => n.IsSelected).Sum(n => (decimal)n.Value);

           AddonTotal = sum;

           
       }

     
       private string applyDisplayformat()  //Changed modifier display format ZC
       {


           StringBuilder sb = new StringBuilder();


            if (Addons.Where(n => n.IsSelected).Any())
            {

                sb.Append(" + ");

                int index = 0;
                foreach (var ad in Addons.Where(m => m.IsSelected))
                {
                    if (index > 0)
                        sb.Append(", + ");
                    sb.Append(ad.Text.Replace("ADD ", ""));
                    index++;

                }

            }

            if (!string.IsNullOrEmpty(CustomComments)) //5/31/16 saa added to concatenate comments.
            {
                if (sb.Length > 0)
                    sb.Append("; ");
                sb.Append(CustomComments);
                //sb.Append("; ");
            }

            if (!string.IsNullOrEmpty(Size))
            {
                if (sb.Length > 0)
                    sb.Append("; ");
                sb.Append(Size);
            }


           if (!string.IsNullOrEmpty(Spice))
           {
               if (sb.Length > 0)
                   sb.Append("; ");
               sb.Append(Spice);
           }

           if (!string.IsNullOrEmpty(Cooked))
           {
               if (sb.Length > 0)
                   sb.Append("; ");
                sb.Append(Cooked);
                //sb.AppendFormat("({0})",Cooked);
           }




         return sb.ToString();
         

       }


       private string _customComments;
       public string CustomComments
       {
           get
           {
              return _customComments;
           }
           set{
               _customComments=value;
           }
       }

    }
}
