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
    public class CustomerVM : NotifyProperty
    {
        public POS_Customer _customer;
        public CustomerVM(POS_Customer Customer = null)
        {
            if (Customer == null)
            {
                _customer = new POS_Customer();
          //      _customer.vOrderType = OrderTypes.Catering; //this is how ordertype is give a initial value .....SAA 
            }

            else
            {
                _customer = Customer;
            //    Subtotal = _customer.Sum(n => n.fLineSubTotal); //this is carry over from OrderVM class

            }


          //  _salexTaxRate = GConfig.POS_Setup.vSalesTax.Value; //this is sample code to retrieve global values for calculation

        }

        //Add CustomerOrderVM to retrieve Customer Orders from POS_OrderHeader/POS_OrderDetails.........SAA
/*
        ObservableCollection<CustomerOrderVM> _orderLines;

        public ObservableCollection<OrderLineVM> OrderLines
        {
            get { return _orderLines ?? (_orderLines = new ObservableCollection<OrderLineVM>(_orderHeader.POS_OrderDetails.Select(n => new OrderLineVM(n.POS_ItemMaster, this, n)))); }
            set
            {
                _orderLines = value;
                OnPropertyChanged("OrderLines");
            }
        }

    */
        public int CustomerID
        {
            get { return _customer.iCustid; }
            set { _customer.iCustid = value; }
        }

        public string CustomerStatus
        {
            get
            {

                return _customer.vCustStatus;
            }
            set
            {
                _customer.vCustStatus = value;

                OnPropertyChanged("CustomerStatus");
            }
        }

        public string CustomerName
        {
            get
            {
                return _customer.vCustName;
            }
            set
            {
                _customer.vCustName = value;

                OnPropertyChanged("CustomerName");
            }
        }
        public string CustomerAddress
        {
            get
            {
                return _customer.vCustAddress1;
            }
            set
            {
                _customer.vCustAddress1 = value;

                OnPropertyChanged("CustomerAddress");
            }
        }
        public string CustomerPhone
        {
            get
            {
                return _customer.vCustPrimaryPh;
            }
            set
            {
                _customer.vCustPrimaryPh = value;

                OnPropertyChanged("CustomerPhone");
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

     
        //Create code for following command in MainVM
        /*
        public DelegateCommand<CustomerVM> SaveOrderCommand
        {
            get
            {
                return MainVM.Main.SaveOrderCommand;
            }
        }
        public DelegateCommand<CustomerVM> PayOrderCommand
        {
            get
            {
                return MainVM.Main.PayOrderCommand;
            }

        }
        public DelegateCommand<CustomerVM> PrintOrderCommand
        {
            get
            {
                return MainVM.Main.PrintOrderCommand;
            }
        }

        public DelegateCommand<CustomerVM> EditOrderCommand
        {
            get
            {
                return MainVM.Main.EditOrderCommand;
            }
        }

        public DelegateCommand<CustomerVM> CancelOrderCommand
        {
            get
            {
                return MainVM.Main.CancelOrderCommand;
            }
        }

    */
        public bool IsPrintEnabled
        {
            get
            {
                if (CustomerStatus != null)
                    return true;
                else return false;
            }
        }

        public bool IsEditEnabled
        {
            get
            {
                if (CustomerStatus == OrderStatuses.Pending)
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

        public bool IsCancelEnabled
        {
            get
            {
                if (CustomerStatus == null ||
                    CustomerStatus == OrderStatuses.Pending)
                    return true;
                else return false;
            }
        }
        public bool IsPayNowEnabled
        {
            get
            {
                if (CustomerStatus == OrderStatuses.Pending)
                    return true;
                else return false;

            }

        }
    }
}
