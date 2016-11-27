using Newtonsoft.Json;
using BaseAppUI.Configuration;
using BaseAppUI.Controls;
using BaseAppUI.Model;
using BaseAppUI.Model.Card;
using BaseAppUI.Sdk.Receipts;
using BaseAppUI.ViewModel.Notifies;
using BaseAppUI.ViewModel.Sections;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseAppUI.ViewModel.Payments
{

    public class ReceiptSplitVM:NotifyProperty
    {
        public string Title { get; set; }
        private decimal _amount;

        public decimal Amount
        {
            get { return _amount; }
            set {
                if ((ValidateAmount==null)||(ValidateAmount!=null && ValidateAmount.Invoke(value)))
                {
                    _amount = value;
                    OnPropertyChanged("Amount");
                }
           
            }
        }
        public bool IsCustomValue { get; set; }

        public Func<decimal, bool> ValidateAmount;

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value;
            OnPropertyChanged("IsActive");
            }
        }
        private bool _isPaid;

        public bool IsPaid
        {
            get { return _isPaid; }
            set { _isPaid = value;
            OnPropertyChanged("IsPaid");
            }
        }
        public SplitPaymentType PaymentType { get; set; }
        public bool IsAmountLocked { get; set; }
    }
    public enum SplitType:int
    {
        Evenly=0,
        ByItems=1,
        ByAmount=2
    }
    public enum SplitPaymentType
    {
        Card,
        Cash
    }
   public class SplitPaymentVM:NotifyProperty,IPayment
    {
       OrderVM _order;
       PaymentSection _prevsection;
       Payment _parent;
       DelegateCommand _cancelSplitCommand;
       private IList<ToggleItem> _selectableSplits;
       private ToggleItem _selectedSplit;
       private IList<ToggleItem> _selectableSplitTypes;
       private ToggleItem _selectedSplitType;
       private SplitPaymentType _paymentType { get; set; }
       private decimal _remainedAmount;
       private DelegateCommand _splitCardPaymentCommand;
       private DelegateCommand _splitCashPaymentCommand;
       private List<KeybordButton> _calculatorButtons;
       private DelegateCommand<KeybordButton> _selectKeybordButton;
       private int? _inScale;
       public SplitPaymentVM(OrderVM order,PaymentSection section,Payment parent)
         {
           _order = order;
           _prevsection = section;
           _parent = parent;
           _remainedAmount = order.AllTotal;

           _paymentType = SplitPaymentType.Card;

           //default 3 split
           var split = SelectableSplits[1];
           split.IsSelected = true;
           _selectedSplit = split;

           //default evenly split
           var splittype = SelectableSplitTypes[0];
           splittype.IsSelected = true;
           _selectedSplitType = splittype;



           PrepareReceipts(order.AllTotal,(int)SelectedSplit.Value,(SplitType)SelectedSplitType.Value);
       }
       public decimal RemainedAmount
       {
           get
           {
               return _remainedAmount;
           }
           set
           {
               _remainedAmount = value;
               OnPropertyChanged("RemainedAmount");
           }
       }


       public DelegateCommand CancelSplitCommand
       {
           get { return _cancelSplitCommand ?? (_cancelSplitCommand = new DelegateCommand(() => {


               _parent.SelectPaymentTypeCommand.Execute(_prevsection);


           })); }
       }

       public SplitPaymentType PaymentType
       {
           get { return _paymentType; }
           set
           {
               _paymentType = value;
               OnPropertyChanged("IsCardPayment");
               OnPropertyChanged("IsCashPayment");
           }
       }
     

       public ToggleItem SelectedSplit
       {
           get
           {
               return _selectedSplit;
           }
           set
           {
               if (!IsSplitInitialized)
               {
                   _selectedSplit = value;
                   PrepareReceipts(_order.AllTotal, (int)_selectedSplit.Value, (SplitType)SelectedSplitType.Value);
               }
           }
       }

       public IList<ToggleItem> SelectableSplits
       {
           get {
               return _selectableSplits ?? (_selectableSplits = Enumerable.Range(2, 9).Select(n => new ToggleItem() {Text=n.ToString(),Value=n }).ToList()); }
       }
     
       public ToggleItem SelectedSplitType
       {
           get
           {
               return _selectedSplitType;
           }
           set
           {
               if (!IsSplitInitialized)
               {
                   _selectedSplitType = value;
                   PrepareReceipts(_order.AllTotal, (int)_selectedSplit.Value, (SplitType)SelectedSplitType.Value);

                   OnPropertyChanged("SelectedSplitType");
               }
             

           }
       }

       public IList<ToggleItem> SelectableSplitTypes
       {
           get
           {
               return _selectableSplitTypes ?? (_selectableSplitTypes = new List<ToggleItem> { 
               new ToggleItem{Text="SPLIT EVENLY",Value=SplitType.Evenly,Content=this},
                //new ToggleItem{Text="SPLIT BY ITEMS",Value=SplitType.ByItems,Content=this},
                 new ToggleItem{Text="SPLIT BY AMOUNT",Value=SplitType.ByAmount,Content=this}
               });
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
                     new KeybordButton{Label="←",Type=KeybordButtonType.Reset},

                 });

           }
       }

       public DelegateCommand<KeybordButton> SelectKeybordButton
       {
           get
           {
               return _selectKeybordButton ?? (_selectKeybordButton = new DelegateCommand<KeybordButton>((e) =>
               {

                   if (_activeSplit.IsAmountLocked) return;



                   switch (e.Type)
                   {

                       case KeybordButtonType.Reset:
                           _activeSplit.Amount = 0.00m;
                           _inScale = null;
                           break;
                       case KeybordButtonType.Scale:
                           _inScale = 2;
                           break;
                       case KeybordButtonType.Double:
                           _activeSplit.Amount *= 100;
                           break;
                       case KeybordButtonType.Numeric:


                           decimal val = (long)_activeSplit.Amount;
                           decimal scale = _activeSplit.Amount - val;

                           if (!_inScale.HasValue)
                           {
                               _activeSplit.Amount = (val * 10) + e.Value + scale;
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

                               _activeSplit.Amount = val + scale;

                           }



                           break;

                   }






               }));
           }

       }

       private IList<ReceiptSplitVM> _receiptSplits;

       public IList<ReceiptSplitVM> ReceiptSplits
       {
           get { return _receiptSplits ?? (_receiptSplits = new List<ReceiptSplitVM>()); }
           set { _receiptSplits = value;
           OnPropertyChanged("ReceiptSplits");
           }
       }


       public bool IsCardPayment
       {
           get
           {
               return _paymentType == SplitPaymentType.Card;
           }
           set { }

       }
       public bool IsCashPayment
       {
           get
           {
               return _paymentType == SplitPaymentType.Cash;
           }
           set { }
       }
       public bool IsAmountSplitType
       {
           get
           {
               return (SplitType)SelectedSplitType.Value==SplitType.ByAmount;
           }
       }
     

       private void PrepareReceipts(decimal amount, int splitnumber, SplitType stype)
       {


           switch (stype)
           {
               case SplitType.Evenly:

                   decimal splitamount = (decimal)amount / splitnumber;

                   var evenlylist = Enumerable
                       .Range(1, splitnumber)
                       .Select(n => new ReceiptSplitVM { Amount = splitamount, Title = string.Format("Receipt #{0}", n) })
                       .ToList();

                   if (evenlylist.Any())
                       evenlylist[0].IsActive = true;


                   ReceiptSplits = evenlylist;
                   _activeSplit = evenlylist[0];
                   break;
               case SplitType.ByItems:

                   ReceiptSplits = new List<ReceiptSplitVM>();
                   _activeSplit = null;
                   break;

               case SplitType.ByAmount:

                   Func<decimal, bool> validateamount = (enteredamount) => {
                       return enteredamount <= RemainedAmount;
                   };

                   var byamountlist = Enumerable
                     .Range(1, splitnumber)
                     .Select(n => new ReceiptSplitVM { Amount = 0.00m,ValidateAmount=validateamount,IsCustomValue=true, Title = string.Format("Receipt #{0}", n) })
                     .ToList();

                   if (byamountlist.Any())
                       byamountlist[0].IsActive = true;

                   ReceiptSplits = byamountlist;
                   _activeSplit = byamountlist[0];
                   break;
           }
       }


       public ReceiptSplitVM _activeSplit { get; set; }
       

       public DelegateCommand SplitCardPaymentCommand
       {
           get
           {
               return _splitCardPaymentCommand ?? (_splitCardPaymentCommand = new DelegateCommand(() =>
               {
                   if (PaymentType != SplitPaymentType.Card)
                   {
                       PaymentType = SplitPaymentType.Card;
                       return;
                   }

                   if (!_order.IsAuthorizing && _activeSplit != null && !_activeSplit.IsPaid)
                   {
                       
                       SaleGenericSwipedModel newsale = new SaleGenericSwipedModel();
                    
                       newsale.Set(_activeSplit.Amount.ToString("#.00"), null); 


                       Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                   {
                       new AuthorizingNotify(this.Authorize, newsale).Show();

                   }));




                   }
                   
                 
                 
                  
                      
                       
               

                   




               }));
           }
       }
       public DelegateCommand SplitCashPaymentCommand
       {
           get
           {
               return _splitCashPaymentCommand ?? (_splitCashPaymentCommand = new DelegateCommand(() =>
               {

                   if (PaymentType != SplitPaymentType.Cash)
                   {
                       PaymentType = SplitPaymentType.Cash;
                       return;
                   }


                   if (!_order.IsAuthorizing && _activeSplit!=null && !_activeSplit.IsPaid)
                   {


                       PaymentType = SplitPaymentType.Cash;
                       _order.IsAuthorizing = true;
                       if (BaseAppUI.Properties.Settings.Default.TipSignModifierSwitch == "SignOnReceipt")
                       {
                           
                               SwitchSplitPaymentProccess(null);
                               new ViewModel.Notifies.ThanksNotify(_order).Show(() =>
                               {
                                   //SwitchSplitPaymentProccess();
                                   _order.IsAuthorizing = false;

                               });
                           
                       }
                       else
                       {
                           new ViewModel.Notifies.TipSignModifier(_order).Show(() =>
                           {
                           SwitchSplitPaymentProccess(null);
                           new ViewModel.Notifies.ThanksNotify(_order).Show(() =>
                           {
                               //SwitchSplitPaymentProccess();
                               _order.IsAuthorizing = false;

                           });
                            });
                       }

                   




                   }













               }));
           }
       }

       public bool Authorize(ISaleModel ccSale)
       {

           if (_activeSplit ==null || _activeSplit.IsPaid) return false;

           _order.IsAuthorizing = true;
           PaymentType = SplitPaymentType.Card;
        
           SaleResponse response = ccSale.Process();




           bool result = (response != null && response.IsValid) ? true : false;
           //bool result = true;


           string errormessage = null;
           if (response != null && !result)
           {
               errormessage = response.ErrorMessage;

           }


           Application.Current.Dispatcher.BeginInvoke(new Action(() =>
           {
               if (result)
               {
                   if (BaseAppUI.Properties.Settings.Default.TipSignModifierSwitch == "SignOnReceipt")
                   {
                       SwitchSplitPaymentProccess(response);
                           new ViewModel.Notifies.ThanksNotify(_order).Show(() =>
                           {
                               //SwitchSplitPaymentProccess();
                               _order.IsAuthorizing = false;

                           });
                       
                   }
                   else
                   {

                       new ViewModel.Notifies.TipSignModifier(_order).Show(() =>
                       {
                           SwitchSplitPaymentProccess(response);
                           new ViewModel.Notifies.ThanksNotify(_order).Show(() =>
                           {
                               //SwitchSplitPaymentProccess();
                               _order.IsAuthorizing = false;

                           });
                       });
                   }

                  
               }

               else new ViewModel.Notifies.TransactionFailedNotify(errormessage, null).Show(() => {
                   _order.IsAuthorizing = false;
               
               });
           }));




           return result;
       }
       private bool _isSplitInitialized;

       public bool IsSplitInitialized
       {
           get { return _isSplitInitialized; }
           set
           {
               _isSplitInitialized = value;
               OnPropertyChanged("IsSplitCancel");
           }
       }
       public bool IsSplitCancel
       {
           get
           {
               return !_isSplitInitialized;
           }
       }
       
       private void SwitchSplitPaymentProccess(SaleResponse response)
       {
           _inScale = null;
           if (!IsSplitInitialized) IsSplitInitialized = true;

           var currentindex = ReceiptSplits.IndexOf(_activeSplit);
          

          _activeSplit.IsPaid = true;
        
          _activeSplit.IsActive = false;
        
          RemainedAmount -= _activeSplit.Amount;

          _activeSplit.PaymentType = PaymentType;

            //payed
            storeSplitPaymentInfo(response);

            if (currentindex + 1 < ReceiptSplits.Count)
          {
              var nextsplit = ReceiptSplits[currentindex + 1];
              nextsplit.IsActive = true;

                _order.PaymentType = Model.OrderPaymentTypes.Split;


                if (IsAmountSplitType && ReceiptSplits.IndexOf(nextsplit) == ReceiptSplits.Count - 1)
                {
                    nextsplit.Amount = RemainedAmount;
                    nextsplit.IsAmountLocked = true;
                }


                _activeSplit = nextsplit;
            }
            else
          {
              _order.PaymentType = Model.OrderPaymentTypes.Split;
              _order.OrderStatus = Model.OrderStatuses.Completed;


              _parent.Release();

             
                      MainVM.Main.SaveOrderCommand.Execute(_order);
                      MainVM.Main.SwitchToView(Model.SectionType.Dashboard);

             
           

           
              
          }

        
          
           

       }
        private void storeSplitPaymentInfo(SaleResponse response)
        {
            if (!_order._orderHeader.iSplitCount.HasValue)
                _order._orderHeader.iSplitCount = ReceiptSplits.Count;


            StringBuilder sb = new StringBuilder();
            if (_order._orderHeader.vSplitPayment != null)
                sb.Append(";");

            sb.Append(PaymentType == SplitPaymentType.Card ? "cash" : "card");
            sb.Append(",");

            if (response != null)
                sb.Append(response.Resp_CardNum);

            sb.Append(",");
            sb.Append(_activeSplit.Amount.ToString("0.##"));
            sb.Append(",");
            sb.AppendFormat("split{0}", (ReceiptSplits.IndexOf(_activeSplit) + 1));

            _order._orderHeader.vSplitPayment += sb.ToString();




            GConfig.DAL.SaveChanges();

        }
    }
}
