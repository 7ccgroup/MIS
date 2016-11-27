using BaseAppUI.Model;
using BaseAppUI.Model.Card;
using BaseAppUI.Sdk;
using BaseAppUI.ViewModel.Notifies;
using BaseAppUI.ViewModel.Payments;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseAppUI.Configuration;


namespace BaseAppUI.ViewModel.Sections
{
   public class Payment:NotifyProperty,ISection
    {
        OrderVM _order;
        public OrderVM Order
        {
            get { return _order; }
            set { _order = value;

            OnPropertyChanged("Order");
            }
        }

      static CreditCardReader _reader;
     
       public Payment(OrderVM order)
       {
           _order = order;

           SelectPaymentTypeCommand.Execute(TopSections[0]);


           InitializeCardReader();
         
       }

    

       async void InitializeCardReader()
       {
       

           await Task.Run(() => {
               try
               {
                   if (_reader == null)
                   {
                       _reader = new CreditCardReader();
                      
                   }
                   _reader.OnCardReadEvent += _reader_OnCardReadEvent;
                   _reader.Connect();
               }
               catch (Exception ex)
               {
                   var error = ex.Message.ToString();
               }
           });
       }


        string _trackdata;
        public void SwipeTransfer(string tracks)
        {
            _trackdata = tracks;
            if (!Order.IsAuthorizing)
            {
                SaleGenericSwipedModel newsale = new SaleGenericSwipedModel();
                newsale.Set (Order.AllTotal.ToString("#.00"), _trackdata); //SAA 5/17===== GConfig.total_amount

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    new AuthorizingNotify(CardPay.Authorize, newsale).Show();

                }));
            }
        }

        void _reader_OnCardReadEvent(Microsoft.PointOfService.Msr obj)
       {
           if (!Order.IsAuthorizing)
           {

               SaleSwipedModel newsale = new SaleSwipedModel();
               newsale.Set(Order.AllTotal.ToString("#.00"), obj);

               Application.Current.Dispatcher.BeginInvoke(new Action(() =>
               {
                   if (IsSplitPaymentView)
                       new AuthorizingNotify(SplitPay.Authorize, newsale).Show();
                   else
                        new AuthorizingNotify(CardPay.Authorize, newsale).Show();

               }));
           }

       }

       private IPayment _context;

       public IPayment Context
       {
           get { return _context; }
           set { _context = value;

           OnPropertyChanged("Context");
                OnPropertyChanged("RightPaneVisibility");
           }
       }

       private CardPayVM _cardPay;

       public CardPayVM CardPay
       {
           get { return _cardPay ?? (_cardPay = new CardPayVM(_order,this)); }
          
       }
    

       private CashPayVM _cashPay;

       public CashPayVM CashPay
       {
           get { return _cashPay ?? (_cashPay = new CashPayVM(_order,this)); }
       }

       private PaymentSection SelectedSection { get; set; }

       private IList<PaymentSection> _topSections;

       public IList<PaymentSection> TopSections
       {
           get { return _topSections ?? (_topSections = new List<PaymentSection> { 
           
                new PaymentSection{Name="CARD",PaymentSectionType=PaymentSectionType.CardPay},
               new PaymentSection{Name="CASH",PaymentSectionType=PaymentSectionType.CashPay}
              
           }); }
           
       }


       private IList<PaymentSection> _modifierSections;

        public IList<PaymentSection> ModifierSections
        {
            get
            {
                return _modifierSections ?? (_modifierSections = new List<PaymentSection>() {

               new PaymentSection{Name="DISCOUNT", PaymentSectionType=PaymentSectionType.Discount}

           });
            }
        }


        public DelegateCommand _returnFromPaymentCommand;
       public DelegateCommand ReturnFromPaymentCommand
       {
           get
           {

               return _returnFromPaymentCommand ?? (_returnFromPaymentCommand = new DelegateCommand(() => {

                   Release();

                   MainVM.Main.ReturnCommand.Execute(null);

               }));
             

           }

       }

       private DelegateCommand<PaymentSection> _selectPaymentTypeCommand;

       public DelegateCommand<PaymentSection> SelectPaymentTypeCommand
       {
           get
           {
               return _selectPaymentTypeCommand ?? (_selectPaymentTypeCommand = new DelegateCommand<PaymentSection>((e) =>
               {

              

               if (e == null) return;
               if (SelectedSection != e)
               {

                   if (SelectedSection != null)
                       SelectedSection.IsSelected = false;


                   SelectedSection = e;
                   SelectedSection.IsSelected = true;




                   switch (e.PaymentSectionType)
                   {
                       case PaymentSectionType.CardPay:
                           Context = CardPay;
                           break;
                       case PaymentSectionType.CashPay:
                           Context = CashPay;
                           break;
                     
                           default: break;
                   }
               }

              

           
           })); }
       }


       private DelegateCommand<PaymentSection> _selectPaymentModifierCommand;

       public DelegateCommand<PaymentSection> SelectPaymentModifierCommand
       {
           get
           {
               return _selectPaymentModifierCommand ?? (_selectPaymentModifierCommand = new DelegateCommand<PaymentSection>((e) =>
               {


                       switch (e.PaymentSectionType)
                       {
                           case PaymentSectionType.Discount:

                               new ViewModel.Notifies.DiscountModifier(_order.Subtotal,_order.Discount,addDiscount).Show();


                               break;
                           default: break;
                       }
                   




               }));
           }
       }


       private void addDiscount(decimal value)
       {

           _order.Discount = value;
           
           _order.RefreshValues();

           if (Context is CardPayVM)
               CardPay.RefreshValues();
           else if (Context is CashPayVM)
               CashPay.RefreshValues();

           if (_order.Id != 0)
           {
                MainVM DiscountMainVm;  //5/18 fix ....for thread exception.
                DiscountMainVm = new MainVM();
               
                //Task.Factory.StartNew(() =>
                //{
                    DiscountMainVm.SaveOrderCommand.Execute(_order);
                    //MainVM.Main.SaveOrderCommand.Execute(_order);

                //});
            }

         

          

       }


       public SectionType PreviousSection { get; set; }
       public SectionType SectionType
       {
           get { return Model.SectionType.Payment; }
       }

       public void Release()
       {
           if (_reader != null)
           {
               _reader.OnCardReadEvent -= _reader_OnCardReadEvent;
               _reader.Disconnect();


           }
       }
       
        private PaymentSection _splitSection;

        public PaymentSection SplitSection
        {
            get
            {
                return _splitSection ?? (_splitSection = new PaymentSection { PaymentSectionType = PaymentSectionType.Split }); }

        }

        private DelegateCommand _splitPaymentCommand;

        public DelegateCommand SplitPaymentCommand
        {
            get
            {
                return _splitPaymentCommand ?? (_splitPaymentCommand = new DelegateCommand(() => 
                {
                    SplitPay = new SplitPaymentVM(_order, SelectedSection, this);
                    Context = SplitPay;

                    if(SelectedSection!=null)
                    {
                        SelectedSection.IsSelected = false;
                        SelectedSection = null;
                    }
                }));
            }
        }

        public bool IsSplitPaymentView
        {
            get { return Context is SplitPaymentVM; }
        }

        public Visibility RightPaneVisibility
        {
            get
            {
                return IsSplitPaymentView ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public SplitPaymentVM SplitPay { get; set; }
    }
}
