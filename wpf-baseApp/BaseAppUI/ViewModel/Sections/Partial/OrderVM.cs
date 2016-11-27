using BaseAppData.Entity;
using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.ViewModel.Notifies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseAppUI.ViewModel.Sections.Partial
{
   public class OrderVM:NotifyProperty
    {
      public POS_OrderHeader _orderHeader;
       public OrderVM(POS_OrderHeader orderHeader=null)
       {
           if (orderHeader == null){
               _orderHeader = new POS_OrderHeader();
               _orderHeader.vOrderType = OrderTypes.DineIn;
               _orderHeader.vOrderStatus = OrderStatuses.Pending;
           }

           else
           {
               _orderHeader = orderHeader;
               Subtotal = _orderHeader.POS_OrderDetails.Sum(n => n.fLineSubTotal);
              if (_orderHeader.dOrderServiceFee > 0) //When order is edit check if fee is included.....
                    ServiceFeeIsSelected = true;
           }


           _salexTaxRate = GConfig.POS_Setup.vSalesTax.Value;
           // OrderType = "Dine-In"; //commented as the order history is showing only dine-in
          
       }

       ObservableCollection<OrderLineVM> _orderLines;

       public ObservableCollection<OrderLineVM> OrderLines
       {
           get { return _orderLines ?? (_orderLines = new ObservableCollection<OrderLineVM>(_orderHeader.POS_OrderDetails.Select(n => new OrderLineVM(n.POS_ItemMaster, this, n) ))); }
           set { _orderLines = value;
           OnPropertyChanged("OrderLines");
           }
       }


       public int Id {
           get { return _orderHeader.Id; }
       }

       public string OrderStatusTitle
       {
           get
           {
               if (Id == 0)
                   return "New Order";
               else return "Order #"+_orderHeader.vOrderNumber; //To display pending order number 
           }
       }

       public bool IsAuthorizing { get; set; }
       public string OrderType
       {
           get { return _orderHeader.vOrderType; }
           set { _orderHeader.vOrderType = value;
           OnPropertyChanged("OrderType");
           OnPropertyChanged("OrderTypeLabel");
           }
       }
        public string OrderTypeTableNumber
        {

            get {
                if (BaseAppUI.Properties.Settings.Default.EnableTableNumber == "Yes" &&
                    _orderHeader.vOrderTableNumber !="" && _orderHeader.vOrderTableNumber !=null)
                    return _orderHeader.vOrderType + " (T-" + _orderHeader.vOrderTableNumber +")";
                else
                    return _orderHeader.vOrderType;
            }
            
        }
        public string OrderTypeLabel
       {
           get { return OrderType.ToUpperInvariant(); }
       }

       public string OrderStatus
       {
           get {
               
               return _orderHeader.vOrderStatus; }
           set
           {
               _orderHeader.vOrderStatus = value;
             
               OnPropertyChanged("OrderStatus");
           }
       }

      

       public string PaymentType
       {
           get
           {
                //Added to show card number after approval......SAA 5/7/16
                if (_orderHeader.vOrderPaymentType == OrderPaymentTypes.Card)
                {
                    int _strLength = 0;
                    string _cardNumber = "";
                    try
                    {
                        _strLength = _orderHeader.vOrderAcct.ToString().Length;
                        _cardNumber = _orderHeader.vOrderAcct.ToString().Substring(_strLength - 4, 4);
                    }
                    catch
                    {
                        return _orderHeader.vOrderPaymentType;
                    }
                    return "Card-" + _cardNumber;
                }
                else
                    return _orderHeader.vOrderPaymentType;
           }
           set
           {
               _orderHeader.vOrderPaymentType = value;
           }
       }
       public DateTime ModifiedDate
       {
           get
           {
             return _orderHeader.dModifiedDate;
           }
       }
        public DateTime CurrentSysDate
        {
            get
            {
                return System.DateTime.Now; //.ToString("MMM dd, yyyy HH:MM"); //_orderHeader.dModifiedDate;
            }
        }
        
       public int OrderNumber
       {
           get
           {
                if (_orderHeader.vOrderNumber >0)
                        return   _orderHeader.vOrderNumber;
                else
                    return GConfig.GenerateLocalOrderNumber();
            }
       }

       public string CustomerName
       {
           get
           {
               if (_orderHeader.Customer != null)
                   return _orderHeader.Customer.vCustName;
               else return null;
           }
       }
        private string _tableNumber;
        public string TableNumber
        {
            get
            {
                if (_tableNumber != null)
                    return _tableNumber;
                else
                return _orderHeader.vOrderTableNumber;
            }
            set
            {
                _tableNumber = value;
                OnPropertyChanged("TableNumber");
            }
        }
       public string CustomerAddress
       {
           get
           {
               if (_orderHeader.Customer != null)
                   return _orderHeader.Customer.vCustAddress1;
               else return null;
           }
       }
       public string CustomerPhone
       {
           get
           {
               if (_orderHeader.Customer != null)
                   return _orderHeader.Customer.vCustPrimaryPh;
               else return null;
           }
       }
       public string PaidVia
       {
           get
           {
               return _orderHeader.vOrderPaymentType;
           }
       }
        //Add by SAA 4/24.
        public string PaymentAuthorization
        {
            get
            {
                return _orderHeader.vOrderPaymentRef;
            }
        }
        public decimal Discount{
           get
           {
               return _orderHeader.dOrderAmtDue;
           }
            set
            {
                _orderHeader.dOrderAmtDue = value;
                OnPropertyChanged("Discount");
            }
       }
        public decimal? TipAmount
        {
            get
            {
                return _orderHeader.dOrderTip;
            }
            set
            {
                _orderHeader.dOrderTip = value;
                OnPropertyChanged("TipAmount");
            }
        }

        public decimal Change
       {
           get
           {
               return 0.0m;
           }
       }
       public object CategoryIcon
       {
           get
           {
               var ordertypevm = MainVM.Main.Orders.AllTypes.FirstOrDefault(n => n._ordertype == this.OrderType);
               if (ordertypevm != null)
                   return ordertypevm.Icon;
               else return null;
           }
       }
    

       private bool _isSelected;

       public bool IsSelected
       {
           get { return _isSelected; }
           set
           {
               _isSelected = value;
               OnPropertyChanged("IsSelected");
           }
       }
   
     // public decimal? ServiceFee { get; set; }

       public decimal? DeliveryCharge { get; set; }

        private decimal _serviceFee;
        public decimal ServiceFee
        {

            get
            {

              
                return _serviceFee;
            }
            set
            {
                _serviceFee = value;
                if (ServiceFeeIsSelected)
                    ServiceFeeTotal = _serviceFee;
                OnPropertyChanged("ServiceFee");
            }
        }

        private decimal _subtotal;
       public decimal Subtotal
       {

           get
           {
                
                ServiceFee = (_subtotal) * BaseAppUI.Properties.Settings.Default.ServiceFee/100;  //5/8/16 saa
               return _subtotal;
           }
           set
           {
               _subtotal = value;
               OnPropertyChanged("Subtotal");
           }
       }


       public decimal TaxSubtotal
       {
           get {
               
               return _orderHeader.dOrderTax;
            }
           set { _orderHeader.dOrderTax = value;
           OnPropertyChanged("TaxSubtotal");
           }
       }


       public decimal AllTotal
       {
           get { return _orderHeader.dOrderAmount; }
           set {
                
               _orderHeader.dOrderAmount = value;
           OnPropertyChanged("AllTotal");
           }
       }
        public decimal? ServiceFeeTotal
        {
            get
            {

                return _orderHeader.dOrderServiceFee;
            }
            set
            {

                _orderHeader.dOrderServiceFee = value;
                OnPropertyChanged("ServiceFeeTotal");
            }
        }
        public decimal? DisplayServiceFee
        {
            get
            {

                return _orderHeader.dOrderServiceFee;
            }
            set { }
          
        }
        public string ServiceFeeLabel
        {
            get {
                
                decimal Sfee = Convert.ToDecimal(BaseAppUI.Properties.Settings.Default.ServiceFee);
                return "GRATUITY ("+Sfee.ToString()+"%)"; }
        }

        public void RefreshValues()
       {

           Subtotal = OrderLines.Sum(n => n.Total);

           TaxSubtotal = (decimal)(Subtotal * SalexTaxRate / 100);

            AllTotal = Subtotal + TaxSubtotal - Discount;

            if (ServiceFeeIsSelected)
                AllTotal += ServiceFee;
            //if  service fee is selected then add service fee to Total
            //+ServiceFeeTotal;


          
          
       }
        public void AddServiceFee()
        {
            AllTotal = AllTotal + ServiceFee;

        }

        public void RemoveServiceFee()
        {
            ServiceFeeTotal = 0;
            AllTotal = AllTotal - ServiceFee;

        }
        private decimal _salexTaxRate;
       public decimal SalexTaxRate {

           get
           {
                //return _salexTaxRate;
                return BaseAppUI.Properties.Settings.Default.Tax_overall; 
                // Show total tax amount on view from settings - Amjad 5/12
           }
           set
           {
               _salexTaxRate = value;
               OnPropertyChanged("SalexTaxRate");
               RefreshValues();
           }
       }
     


       private DelegateCommand<OrderLineVM> _selectLineItemCommand;

       public DelegateCommand<OrderLineVM> SelectLineItemCommand
       {
           get { return _selectLineItemCommand ?? (_selectLineItemCommand = new DelegateCommand<OrderLineVM>((e) => {


               new LineItemModifier(e).Show(null);
             
           
           })); }
          
       }

       public void ResetUnSavedChanges()
       {
           if (_orderHeader.Id != 0)
           {
               //GConfig.DAL.StateChange(_orderHeader, System.Data.Entity.EntityState.Unchanged);

               //foreach(var detail in _orderHeader.POS_OrderDetails.Where(n=>n.Id!=0))
               //    GConfig.DAL.StateChange(detail, System.Data.Entity.EntityState.Unchanged);

               
               //GConfig.DAL.Refresh(_orderHeader);

               GConfig.DAL.Rollback(_orderHeader);

               foreach (var n in _reservedLines)
                   GConfig.DAL.Rollback(n);

               _orderLines = null;
           }

       }

       private IList<POS_OrderDetails> _reservedLines;

       public IList<POS_OrderDetails> ReservedLines
       {
           get { return _reservedLines??(_reservedLines=new List<POS_OrderDetails>()); }
           set
           {
               _reservedLines = value;
           }
       }

        private DelegateCommand _serviceCommand;
        public DelegateCommand ServiceFeeCommand
        {
            get
            {
                return _serviceCommand ?? (_serviceCommand = new DelegateCommand (() =>
                {
                    if (ServiceFeeIsSelected)
                        AddServiceFee();
                    else
                        RemoveServiceFee();


                }));
            }

        }
        private bool _serviceFeeIsSelected;
        public bool ServiceFeeIsSelected
        {
            get
            {
                return _serviceFeeIsSelected;
            }
            set { 
                _serviceFeeIsSelected = value;
                OnPropertyChanged("ServiceFeeIsSelected");
            }

        }

        public DelegateCommand<OrderVM> SaveOrderCommand
       {
           get
           {
               return MainVM.Main.SaveOrderCommand;
           }
       }
       public DelegateCommand<OrderVM> PayOrderCommand
       {
           get
           {
              return MainVM.Main.PayOrderCommand;
           }

       }
       public DelegateCommand<OrderVM> PrintOrderCommand
       {
           get
           {
              return MainVM.Main.PrintOrderCommand;
           }
       }


        public DelegateCommand<OrderVM> KitchenPrintCommand
        {
            get
            {
                return MainVM.Main.KitchenPrintCommand;
            }
        }

     
        
        public DelegateCommand<OrderVM> EditOrderCommand
       {
           get
           {
                
               return MainVM.Main.EditOrderCommand;
           }
       }

        // Add Tip Button - Amjad 5/10
        public DelegateCommand<OrderVM> AddTipCommand
        {
            get
            {

                return MainVM.Main.AddTipCommand;
            }
        }


        // Refund Button - Amjad 5/13
        public DelegateCommand<OrderVM> RefundCommand
        {
            get
            {

                return MainVM.Main.RefundCommand;
            }
        }


        public DelegateCommand<OrderVM> CancelOrderCommand
       {
           get
           {
               return MainVM.Main.CancelOrderCommand;
           }
       }
      

       public bool IsPrintEnabled
       {
           get
           {
               if (OrderStatus !=null)    
                   return true;
               else return false;
           }
       }

        // Enable Tip button for completed orders only - Amjad
        public bool IsTipEnabled
        {
            get
            {
                if (OrderStatus == OrderStatuses.Completed && PaymentType!=OrderPaymentTypes.Cash)
                    return true;
                else return false;
            }
        }

        // Added by Amjad 5/13
        public bool IsRefundEnabled
        {
            get
            {
                if (OrderStatus == OrderStatuses.Completed)
                    return true;
                else return false;
            }
        }



        public bool IsChangeEnabled
        {
            get
            {
                return !IsTipEnabled;
            }
        }

        public bool IsEditEnabled
       {
           get
           {
               if (OrderStatus == OrderStatuses.Pending)
                   return true;
               else return false;
           }
       }
       public Visibility EditVisibility
       {
           get
           {
               if (IsEditEnabled)
                   return Visibility.Visible;
               else return Visibility.Collapsed;
           }
       }

        // Tip visibility
        public Visibility TipVisibility
        {
            get
            {
                if (IsTipEnabled && PaymentType != OrderPaymentTypes.Cash)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        // Pay Now visibility - Amjad 5/13
        public Visibility PayNowVisibility
        {
            get
            {
                if (OrderStatus == OrderStatuses.Pending)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        // Refund visibility - Amjad 5/13
        public Visibility RefundVisibility
        {
            get
            {
                //if (OrderStatus == OrderStatuses.Completed)
                    if (OrderStatus == OrderStatuses.Completed && PaymentType != OrderPaymentTypes.Cash)
                        return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }


        // Cancel button visibility - Amjad 5/18 - changes status to cancelled for pending orders
        public Visibility CancelVisibility
        {
            get
            {
                if (OrderStatus == null)
                //if (OrderStatus == OrderStatuses.Pending)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        public Visibility ChangeVisibility
        {
            get
            {
                //if (IsChangeEnabled && PaymentType != OrderPaymentTypes.Card)
                //    return Visibility.Visible;
                //else return Visibility.Collapsed;
                return Visibility.Collapsed;
            }
        }

        // Enable only for cash - Amjad 5/18
        public bool IsCancelEnabled
       {
           get
           {
               if (OrderStatus == OrderStatuses.Pending)
                   return true;
               else return false;
           }
       }
        public bool IsPayNowEnabled
        {
            get
            {
                if (OrderStatus == OrderStatuses.Pending)
                    return true;
                else return false;
            }
        }
    }
}
