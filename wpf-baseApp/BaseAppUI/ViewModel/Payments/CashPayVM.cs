using BaseAppUI.Model;
using BaseAppUI.ViewModel.Notifies;
using BaseAppUI.ViewModel.Sections;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseAppUI.ViewModel.Payments
{
    public enum KeybordButtonType
    {
        Numeric,
        Scale,
        Reset,
        Double,
    }
    public class KeybordButton
    {
        public decimal Value { get; set; }
        public string  Label { get; set; }
        public KeybordButtonType Type { get; set; }

    }

    public class QuickAmount:NotifyProperty
    {
        public decimal Value { get; set; }
        public string DisplayFormat { get; set; }

        public string DisplayValue
        {
            get {

                return Value.ToString(DisplayFormat, CultureInfo.CreateSpecificCulture("en-US"));
                
                 }
        }

        public void ChangeValue(decimal v) {

            Value = v;
            OnPropertyChanged("DisplayValue");
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value;
            OnPropertyChanged("IsSelected");
            }
        }
    }

   public class CashPayVM:NotifyProperty, IPayment
    {
         OrderVM _order;
        Payment _parent;
         public CashPayVM(OrderVM order,Payment payment)
         {
           _order = order;
            _parent = payment;
           SetSelectableAmounts();
       
       }

       private void SetSelectableAmounts(){

           decimal am1 = _order.AllTotal;
           decimal am0 = ((int)(am1 / 10)) * 10;

           decimal k =am1 - am0;

           decimal d = k > 5 ? 10 : 5;


           decimal am2 = am0 + d;
           decimal am3 = am2 + 5;

           decimal am4=100m;

           while (am4<am3)
           {
               am4 += 20;
           }



           _selectableAmounts = new List<QuickAmount> { 
               new QuickAmount { Value = am1,DisplayFormat="C2" }, 
               new QuickAmount { Value = am2,DisplayFormat="C0" }, 
               new QuickAmount { Value = am3,DisplayFormat="C0" }, 
               new QuickAmount { Value = am4,DisplayFormat="C0" } };

           SelectAmountCommand.Execute(_selectableAmounts[0]);
       }

         public decimal AllTotal
         {

             get
             {
                 return _order.AllTotal;
             }
         }

         private decimal _tendered;

         public decimal Tendered
         {
             get { return _tendered; }
             set {

                 if (value < 999999m)
                 {
                     _tendered = value;
                     OnPropertyChanged("Tendered");

                     OnPropertyChanged("Change");
                 }
                 
                
            
             }
         }

        private Visibility _splitButtonVisible;
        public Visibility SplitButtonVisible
        {
            get
            {
                if (BaseAppUI.Properties.Settings.Default.POSversion != 1)
                    _splitButtonVisible = Visibility.Visible;
                else
                    _splitButtonVisible = Visibility.Hidden;
                return _splitButtonVisible;
            }
        }


        public decimal Change
         {
             get {

                 var val = Tendered - AllTotal;

                 if (val >= 0)
                 {
                     IsValidTender = true;
                     return val;
                 }
                 else 
                 {
                     IsValidTender = false;
                     return 0m;
                 }
           
                 
                  }
             
         }

         private bool _isValidTender;

         public bool IsValidTender
         {
             get { return _isValidTender; }
             set
             {
                 _isValidTender = value;
                 OnPropertyChanged("IsValidTender");
             }
         }
         

         public int? InScale { get; set; }

         private List<QuickAmount> _selectableAmounts;

         public List<QuickAmount> SelectableAmounts
         {
             get { return _selectableAmounts; }
             set { _selectableAmounts = value; }
         }

         private KeybordButton _resetTender;

         public KeybordButton ResetTender
         {
             get { return _resetTender ?? (_resetTender = new KeybordButton {Type=KeybordButtonType.Reset}); }
             
         }


         private List<KeybordButton> _calculatorButtons;

         public List<KeybordButton> CalculatorButtons
         {
             get {

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


         private QuickAmount SelectedQuickAmount { get; set; }

         private DelegateCommand<QuickAmount> _selectAmountCommand;

         public DelegateCommand<QuickAmount> SelectAmountCommand
         {
             get
             {
                 return _selectAmountCommand ?? (_selectAmountCommand = new DelegateCommand<QuickAmount>((e) =>
                 {

                 Tendered = e.Value;

                 if (e == null) return;
                 if (SelectedQuickAmount != e)
                 {

                     if (SelectedQuickAmount != null)
                         SelectedQuickAmount.IsSelected = false;


                     SelectedQuickAmount = e;
                     SelectedQuickAmount.IsSelected = true;

                 }

                
                

             
             })); }
         }

         private DelegateCommand<KeybordButton> _selectTenderButton;

         public DelegateCommand<KeybordButton> SelectTenderButton
         {
             get { return _selectTenderButton ?? (_selectTenderButton = new DelegateCommand<KeybordButton>((e) => {


                

                 switch (e.Type) {
                 
                     case KeybordButtonType.Reset:
                         Tendered = 0m;
                         InScale = null;
                         break;
                     case KeybordButtonType.Scale:
                         InScale = 2;
                         break;
                     case KeybordButtonType.Double:
                         Tendered *= 100;
                         break;
                     case KeybordButtonType.Numeric:


                           decimal val = (long)Tendered;
                           decimal scale = (decimal)Tendered - val;

                         if (!InScale.HasValue){
                             Tendered = (val * 10) + e.Value +scale;
                         }
                         else
                         {

                             if (InScale.Value == 2) {
                                 scale = e.Value / 10;
                                 InScale = 1;
                             }
                             else
                             {
                                 decimal scale1 = (long)(scale * 10);
                                 scale = (decimal)((scale1 * 10) + e.Value)/100;
                                 InScale = 2;

                             }

                             Tendered = val + scale;

                         }
                


                         break;
                 
                 }



                 if (SelectedQuickAmount != null)
                 {
                     SelectedQuickAmount.IsSelected = false;
                     SelectedQuickAmount = null;


                 }


             })); }
            
         }


         private DelegateCommand _completeCashPaymentCommand;

         public DelegateCommand CompleteCashPaymentCommand
         {
             get { return _completeCashPaymentCommand ?? (_completeCashPaymentCommand = new DelegateCommand(() => {
                 _order.PaymentType = Model.OrderPaymentTypes.Cash;
                 _order.OrderStatus = Model.OrderStatuses.Completed;
                 try
                 {
                     int result = Sdk.EloCashDrawer.Open(); 
                     // Open Elo CashDrawer at the end of sale, if connected - Amjad 6/1
                 }
                 catch (Exception ex)
                 {
                     //System.Windows.MessageBox.Show("Elo not connected.");
                     //Do nothing
                 }

                 new ThanksNotify(_order).Show(() =>
                 {
                     ((BaseAppUI.ViewModel.Sections.Payment)MainVM.Main.Content).Release();

                     MainVM.Main.SwitchToView(Model.SectionType.Dashboard);

                 });




             })); }
         }


         public void RefreshValues()
         {

             OnPropertyChanged("AllTotal");

             Tendered = AllTotal;
             SelectableAmounts[0].ChangeValue(Tendered);

             SelectAmountCommand.Execute(SelectableAmounts[0]);
         }
        public DelegateCommand SplitPaymentCommand
        {
            get
            {
                return _parent.SplitPaymentCommand;
            }
        }
    }
}
