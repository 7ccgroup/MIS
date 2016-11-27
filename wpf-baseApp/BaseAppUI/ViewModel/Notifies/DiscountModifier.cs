using BaseAppUI.ViewModel.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.ViewModel.Notifies
{
    public enum DiscountActionType
    {
        Cancel,
        Done,
        Reset
    }

    public enum DiscountConverterType
    {
        Regular,
        Percent

    }

    public class DiscountConverter : NotifyProperty
    {
        public DiscountConverterType Type { get; set; }  
        public string Label { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value;
            OnPropertyChanged("IsSelected");
            }
        }

    }
   public class DiscountModifier:NotifyBase
    {
       private int? _inScale;
       private KeybordButton _resetDiscount;
       private DelegateCommand<DiscountActionType> _discountCommand;
       private List<KeybordButton> _calculatorButtons;
       private List<DiscountConverter> _discountConverters;
       private DelegateCommand<DiscountConverter> _selectDiscountConverterCommand;
       private DelegateCommand<KeybordButton> _selectKeybordButton;

       private Action<decimal> _adddiscoundaction;
       private decimal _subtotal;

       public string Title { get; set; }

       public bool IsAsingnedToTip { get; set; }
       public DiscountModifier(decimal subtotal,decimal recentdiscount,Action<decimal> adddiscoundaction)
       {
           _subtotal = subtotal;
           _adddiscoundaction = adddiscoundaction;
           _discountValue = recentdiscount;
           base.EnableMask = true;

           SelectDiscountConverterCommand.Execute(DiscountConverters[0]);

       }

       private decimal _discountValue;

       public decimal DiscountValue
       {
           get { return _discountValue; }
           set {

               if (value < 999999m)
               {
                   _discountValue = value;

                   OnPropertyChanged("DiscountValue");
               }
           }
       }
      
   

       public List<KeybordButton> CalculatorButtons
       {
           get
           {

               return _calculatorButtons ?? (_calculatorButtons = new List<KeybordButton>() { 
                 
                     new KeybordButton{Label="1",Value=1},
                     new KeybordButton{Label="2",Value=2},
                     new KeybordButton{Label="3",Value=3},
                     new KeybordButton{Label="4",Value=4},
                     new KeybordButton{Label="5",Value=5},
                     new KeybordButton{Label="6",Value=6},
                     new KeybordButton{Label="7",Value=7},
                     new KeybordButton{Label="8",Value=8},
                     new KeybordButton{Label="9",Value=9},
                     new KeybordButton{Label=".",Type=KeybordButtonType.Scale},
                     new KeybordButton{Label="0",Value=0},
                     new KeybordButton{Label="00",Type=KeybordButtonType.Double},

                 });

           }
       }

       public List<DiscountConverter> DiscountConverters
       {
           get
           {
             return _discountConverters??(_discountConverters = new List<DiscountConverter>() { 
               
                   new DiscountConverter{Type=DiscountConverterType.Regular,Label="$"},
                   new DiscountConverter{Type=DiscountConverterType.Percent,Label="%"}

               }) ;
           }
       }

       public KeybordButton ResetDiscount
       {
           get { return _resetDiscount ?? (_resetDiscount = new KeybordButton { Type = KeybordButtonType.Reset }); }

       }

       public DelegateCommand<DiscountActionType> DiscountCommand
       {
           get
           {
               return _discountCommand ?? (_discountCommand = new DelegateCommand<DiscountActionType>((e =>
               {

                   switch (e)
                   {
                       case DiscountActionType.Cancel:
                           this.CloseCommand.Execute(null);
                           break;

                       case DiscountActionType.Done:


                           var val = Validate();

                           if (val==-1m)
                               return;





                           if (_adddiscoundaction != null)
                               _adddiscoundaction.Invoke(val);
                           this.CloseCommand.Execute(null);


                           break;
                   }

               })));
           }
       }

       private decimal Validate()
       {

           switch(SelectedConverter.Type)
           { 
               
               case DiscountConverterType.Regular:

                   if (_subtotal > DiscountValue)
                       return DiscountValue;
                   else return -1m;
               case DiscountConverterType.Percent:

                   var discount = (decimal)(_subtotal * DiscountValue / 100);
                   if (_subtotal > discount)
                       return discount;
                   else return -1m;

               default: return -1m;
           
           }
         
       }

       public DelegateCommand<KeybordButton> SelectKeybordButton
       {
           get
           {
               return _selectKeybordButton ?? (_selectKeybordButton = new DelegateCommand<KeybordButton>((e) =>
               {




                   switch (e.Type)
                   {

                       case KeybordButtonType.Reset:
                           DiscountValue = 0m;
                           _inScale = null;
                           break;
                       case KeybordButtonType.Scale:
                           _inScale = 2;
                           break;
                       case KeybordButtonType.Double:
                           DiscountValue *= 100;
                           break;
                       case KeybordButtonType.Numeric:


                           decimal val = (long)DiscountValue;
                           decimal scale = (decimal)DiscountValue - val;

                           if (!_inScale.HasValue)
                           {
                               DiscountValue = (val * 10) + e.Value + scale;
                           }
                           else
                           {

                               if (_inScale.Value == 2)
                               {
                                   scale = e.Value / 10;
                                   _inScale = 1;
                               }
                               else
                               {
                                   decimal scale1 = (long)(scale * 10);
                                   scale = (decimal)((scale1 * 10) + e.Value) / 100;
                                   _inScale = 2;

                               }

                               DiscountValue = val + scale;

                           }



                           break;

                   }






               }));
           }

       }



       private DiscountConverter _selectedConverter;

       public DiscountConverter SelectedConverter
       {
           get { return _selectedConverter; }
           set { _selectedConverter = value;
           OnPropertyChanged("SelectedConverter");
           }
       }

       public DelegateCommand<DiscountConverter> SelectDiscountConverterCommand
       {
           get
           {
               return _selectDiscountConverterCommand ?? (_selectDiscountConverterCommand = new DelegateCommand<DiscountConverter>((e) => {

                   if (SelectedConverter != e)
                   {

                       if (SelectedConverter != null)
                           SelectedConverter.IsSelected = false;


                       SelectedConverter = e;
                       SelectedConverter.IsSelected = true;

                   }

               }));
           }
       }

    }



}
