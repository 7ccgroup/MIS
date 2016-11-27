using BaseAppUI.Model.Card;
using BaseAppUI.Sdk;
using BaseAppUI.ViewModel.Notifies;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.Sdk.Receipts;
using BaseAppUI.ViewModel.Sections;

namespace BaseAppUI.ViewModel.Payments
{
    public enum CardPayDetailType
    {
        
        CardNumber,
        ExpiryDate,
        CVV,
        //ZipCode
    }

    public enum KeyboardButtonType
    {
        Numeric,
        Back,
        Check
    }

    public class KeyboardButton
    {
        public decimal Value { get; set; }
        public string Label { get; set; }
        public KeyboardButtonType Type { get; set; }
    }

    public class CardPayDetail:NotifyProperty
    {
        private string _validationMessage;
        private string _value;
        private DelegateCommand _reset;
        private bool _isValidated;
        private bool _hasValue;
        private bool _isSelected;

        public CardPayVM Parent { get; set; }
      

       

        public CardPayDetailType CardPayDetailType { get; set; }
        public string Label { get; set; }


        private string _displayText;

        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                _displayText = value;

                OnPropertyChanged("DisplayText");
            }
        }
        public string Value
        {
            get { return _value; }
            set { 
                
                _value = value;
                //OnPropertyChanged("Value");
                HasValue = !string.IsNullOrEmpty(_value);

                if (HasValue)
                {
                    if (FormatDisplayText != null)
                        DisplayText = FormatDisplayText(value);
                    else DisplayText = value;
                }
                else DisplayText = null;

                

                if (Validate != null)
                    Validate.Invoke(this);
       

            }
        }
        public string ValidationMessage
        {
            get { return _validationMessage; }
            set { _validationMessage = value;

            OnPropertyChanged("ValidationMessage");
            }
        }
        public bool IsNotValidated
        {
            get { return _isValidated; }
            set
            {
                _isValidated = value;
                OnPropertyChanged("IsNotValidated");
            }
        }


        public bool HasValue
        {
            get { return _hasValue; }
            set { _hasValue = value;

            OnPropertyChanged("HasValue");
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


        public DelegateCommand Reset
        {
            get { return _reset ?? (_reset = new DelegateCommand(() => {

                Value = null;

               


                Parent.SelectCardPayDetailCommand.Execute(this);

            })); }
        }

        public Action<CardPayDetail> Validate = null;

        public Func<string, string> FormatDisplayText = null;

       

    }


   public class CardPayVM:NotifyProperty,IPayment
    {
       OrderVM _order;
       private IList<CardPayDetail> _cardDetails;
       private IList<KeyboardButton> _keyboardButtons;
       private DelegateCommand<KeyboardButton> _selectKeyboardButtonCommand;
       private DelegateCommand<CardPayDetail> _selectCardPayDetailCommand;
       private DelegateCommand _authorizeCardPaymentCommand;
        Payment _parent;
      



        public CardPayVM(OrderVM order,Payment parent)
         {
           _order = order;

            _parent = parent;
          
       }

      

       public decimal AllTotal
       {
           get
           {
               return _order.AllTotal;
           }
       }

       public void RefreshValues() {


           OnPropertyChanged("AllTotal");
          
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


        public IList<CardPayDetail> CardDetails
       {
           get {
               if (_cardDetails == null) {

                   CardNumber = new CardPayDetail
                   {
                       Parent = this,
                       CardPayDetailType = CardPayDetailType.CardNumber,
                       Label = "CARD NUMBER",
                       Validate = ValidateCardNumber,
                       FormatDisplayText = FormatCardNumberDisplayText,

                       //Value = "182014504561123"
                   };

                   ExpiryDate = new CardPayDetail
                   {
                       Parent = this,
                       CardPayDetailType = CardPayDetailType.ExpiryDate,
                       Label = "EXPIRY DATE",
                       Validate = ValidateCardExpiryDate,
                       FormatDisplayText = FormatCardExpiryDateDisplayText
                       

                   };

                   CVV = new CardPayDetail
                   {
                       Parent = this,
                       CardPayDetailType = CardPayDetailType.CVV,
                       Label = "CVV",
                       Validate = ValidateCardCVV,
                   };

                   _cardDetails = new List<CardPayDetail> { 
                       CardNumber,
                       ExpiryDate,
                       CVV
                   };
               
                    //  new CardPayDetail {Parent=this, CardPayDetailType = CardPayDetailType.ZipCode, Label = "ZIP CODE",
                    //     Validate=ValidateCardZipCode,
                    
                    //}
               }



               return _cardDetails;
           
           }
           
       }

      


       public IList<KeyboardButton> KeyboardButtons
       {
           get { return _keyboardButtons ?? (_keyboardButtons = new List<KeyboardButton> { 
           
               new KeyboardButton{Label="1",Value=1},
               new KeyboardButton{Label="2",Value=2},
               new KeyboardButton{Label="3",Value=3},
               new KeyboardButton{Label="4",Value=4},
               new KeyboardButton{Label="5",Value=5},
               new KeyboardButton{Label="6",Value=6},
               new KeyboardButton{Label="7",Value=7},
               new KeyboardButton{Label="8",Value=8},
               new KeyboardButton{Label="9",Value=9},
               new KeyboardButton{Label="←",Type=KeyboardButtonType.Back},
               new KeyboardButton{Label="0",Value=0},
               //new KeyboardButton{Label="✓",Type=KeyboardButtonType.Check},

           }); }
       }


       public DelegateCommand<KeyboardButton> SelectKeyboardButtonCommand
       {
           get
           {
               return _selectKeyboardButtonCommand ?? (_selectKeyboardButtonCommand = new DelegateCommand<KeyboardButton>((e) =>
               {

                   if (SelectedDetail == null && e.Type!=KeyboardButtonType.Check) return;


                   switch (e.Type)
                   {
                       case KeyboardButtonType.Numeric:
                           SelectedDetail.Value += e.Value.ToString();
                           break;

                       case KeyboardButtonType.Back:

                           string val = SelectedDetail.Value;
                           if (!string.IsNullOrEmpty(val))
                           {
                               val = val.Substring(0, val.Length - 1);
                               SelectedDetail.Value = val;

                           }

                           break;

                       case KeyboardButtonType.Check:

                           foreach (var d in CardDetails.Where(n=>n.Validate!=null))
                               d.Validate(d);


                           break;

                   }






               }));
           }

       }

       private CardPayDetail SelectedDetail { get; set; }

       public DelegateCommand<CardPayDetail> SelectCardPayDetailCommand
       {
           get
           {
               return _selectCardPayDetailCommand ?? (_selectCardPayDetailCommand = new DelegateCommand<CardPayDetail>((e) =>
               {

                   if (e == null) return;
                   if (SelectedDetail != e)
                   {

                       if (SelectedDetail != null)
                           SelectedDetail.IsSelected = false;


                       SelectedDetail = e;
                       SelectedDetail.IsSelected = true;

                   }

                   foreach (var m in CardDetails.Where(n => n.Value == null))
                       m.IsNotValidated = false;



               }));
           }

       }


       #region Validations
       private void ValidateCardNumber(CardPayDetail detail)
       {

           string creditCardNumber = detail.Value;
          

           var result =!string.IsNullOrEmpty(creditCardNumber) && creditCardNumber.Length == 16;


           detail.IsNotValidated = !result;
           if (!result)
               detail.ValidationMessage = "16 digits required";
     

           
          
       }
       private void ValidateCardCVV(CardPayDetail detail)
       {

           string cvv = detail.Value;


           var result = !string.IsNullOrEmpty(cvv) && cvv.Length == 3;


           detail.IsNotValidated = !result;
           if (!result)
               detail.ValidationMessage = "3 digits required";




       }
       private void ValidateCardExpiryDate(CardPayDetail detail)
       {

           string expirydate = detail.Value;


           var result = !string.IsNullOrEmpty(expirydate) && expirydate.Length == 4;


           detail.IsNotValidated = !result;
           if (!result)
               detail.ValidationMessage = "4 digits required";




       }
       private void ValidateCardZipCode(CardPayDetail detail)
       {

           string zipcode = detail.Value;


           var result = !string.IsNullOrEmpty(zipcode) && zipcode.Length == 5;


           detail.IsNotValidated = !result;
           if (!result)
               detail.ValidationMessage = "5 digits required";




       }

       #endregion

       #region DisplayFormats

       public string FormatCardNumberDisplayText(string value)
       {
           StringBuilder sb = new StringBuilder();
                  var vals= value.ToCharArray();
                  for (int i = 0; i < vals.Length; i++)
                  {

                      if (i!=0 && (i % 4) == 0)
                          sb.Append(" ");

                      sb.Append(vals[i]);

                  }


                      return sb.ToString();
       }

       public string FormatCardExpiryDateDisplayText(string value)
       {
           StringBuilder sb = new StringBuilder();
           var vals = value.ToCharArray();
           for (int i = 0; i < vals.Length; i++)
           {

               if (i==2)
                   sb.Append(" / ");

               sb.Append(vals[i]);

           }


           return sb.ToString();
       }
       #endregion

       private SaleKeyedModel _saleKeyed;

       public SaleKeyedModel SaleKeyed
       {
           get { return _saleKeyed ?? (_saleKeyed = new SaleKeyedModel()); }
       }

       public DelegateCommand AuthorizeCardPaymentCommand
       {
           get
           {
               return _authorizeCardPaymentCommand ?? (_authorizeCardPaymentCommand = new DelegateCommand(() =>
               {


                   SaleKeyed.Set(CardNumber.Value, ExpiryDate.Value, CVV.Value, AllTotal.ToString("#.00"));
                   //_order._orderHeader.vOrderPaymentType = OrderPaymentTypes.Card;
                   new AuthorizingNotify(Authorize, SaleKeyed).Show();

               }));
           }
       }

       public CardPayDetail CardNumber { get; private set; }
     
       public CardPayDetail ExpiryDate{ get; private set; }

       public CardPayDetail CVV { get; private set; }



        private bool LinkCreditCardResponseToOrder(SaleResponse CCsaleResponse)
        {
            _order._orderHeader.vOrderAcct = CCsaleResponse.Resp_CardNum;
            _order._orderHeader.vOrderSubAcct = CCsaleResponse.Resp_TxnId;
            _order._orderHeader.vOrderRef = CCsaleResponse.Resp_ApprovalCode;
            _order._orderHeader.vOrderPaymentRef = CCsaleResponse.Resp_Msg;

            return true;

        }

        public bool Authorize(ISaleModel ccSale)
       {
          _order.IsAuthorizing = true;
            SaleResponse response = ccSale.Process();
            GConfig.response2 = response;

            //Save Response to Order on successful response. //Add by SAA 5/6/16
           if (response.Resp_Msg == "APPROVAL")
               LinkCreditCardResponseToOrder(response);


            bool result = (response != null && response.IsValid) ? true : false;
            //bool result = true;


           string errormessage = null;
           if (response != null && !result)
           {
               errormessage = response.ErrorMessage;

           }
           

           Application.Current.Dispatcher.BeginInvoke(new Action(() =>
           {
               if (result){

                   _order.PaymentType = Model.OrderPaymentTypes.Card;
                    _order.OrderStatus = Model.OrderStatuses.Completed;

                    var _h = _order._orderHeader;

                    _h.dOrderTax = _order.TaxSubtotal;
                    _h.dOrderAmount = _order.AllTotal;
                    _h.TxnId = response.Resp_TxnId;
                    _h.vApprovalCode = response.Resp_ApprovalCode;


                   //new ViewModel.Notifies.PaymentCompletedNotify().Show(() =>         // Commenting this to remove payment complete screen. Amjad 5/15
                   //{

                       ((BaseAppUI.ViewModel.Sections.Payment)MainVM.Main.Content).Release();

                   if(BaseAppUI.Properties.Settings.Default.TipSignModifierSwitch == "SignOnReceipt")
                   {

                           ReceiptPrinter.Print(_order); // Print Merchant Copy Automatically for signature & tip - Amjad 5/15

                           new ViewModel.Notifies.ThanksNotify(_order).Show(() => {

                               MainVM.Main.SwitchToView(Model.SectionType.Dashboard);

                           });

                   }
                   else
                   {
                       new ViewModel.Notifies.TipSignModifier(_order).Show(() =>
                       {

                           new ViewModel.Notifies.ThanksNotify(_order).Show(() => {

                               MainVM.Main.SwitchToView(Model.SectionType.Dashboard);

                           });
                       });
                   }
                     
                   //});
               }
                   
               else new ViewModel.Notifies.TransactionFailedNotify(errormessage,null).Show();
           }));



            _order.IsAuthorizing = false;
          
           return result;
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
