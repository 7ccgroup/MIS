using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.ViewModel.Notifies
{
    public enum OrderTypeModifierButton
    {
        Cancel,
        Done
    }
   
    public class OrderTypeModifierValue:NotifyProperty
    {
        private string _value;
        public string Value { get { return _value; } set
            { _value = value; } }

        public string Label
        {
            get
            {
                return Value.ToUpperInvariant();
            }
            
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
   public class OrderTypeModifier:NotifyBase
    {
       private DelegateCommand<OrderTypeModifierButton> _orderTypeModifierCommand;
       private DelegateCommand<OrderTypeModifierValue> _selectOrderTypeCommand;
        private DelegateCommand<OrderTypeModifierValue> _selectCustomerCommand;

        Action<string, string, string, string, string, string, string> _addCustomer;

        private IList<OrderTypeModifierValue> _selectableOrderTypes;
       OrderVM _order;
       private OrderTypeModifierValue SelectedOrderType { get; set; }

        string _phone;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            
            }
        }
        private string _address1;
        public string Address1
        {
            get
            {
                return _address1;
            }
            set { _address1 = value;
                OnPropertyChanged("Address1");
            }
        }
        private string _address2;
        public string Address2
        {
            get
            {
                return _address2;
            }
            set { _address2 = value;
                OnPropertyChanged("Address2");
            }
        }
        private string _city;
        public string City
        {
            get
            {
                return _city;

            }
            set { _city = value;
                OnPropertyChanged("City");
            }
        }
        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set { _state = value;
                OnPropertyChanged("State");
            }
        }
        private string _zipcode;
        public string ZipCode
        {
            get
            {
                return _zipcode;
            }
            set { _zipcode = value;
                OnPropertyChanged("ZipCode");
            }
        }
        //public string Phone { get; set; }
        //public string Name { get; set; }
        //public string Address1 { get; set; }
        //public string Address2 { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }

        //public string ZipCode { get; set; }
        public bool IsAdressVisible
       {
           get
           {
               if (SelectedOrderType.Value == OrderTypes.DineIn)
                   return false;
               else return true;
           }
       }
       public bool boolDineIn, boolToGo, boolDelivery, boolCatering;
        public OrderTypeModifier(OrderVM order, Action<string, string, string, string, string, string, string> AddCustomer)
       {
           _order=order;
            SelectedOrderType = new OrderTypeModifierValue(); 
            SelectedOrderType= SelectableOrderTypes.FirstOrDefault(n=>n.Value == _order.OrderType); //Select from the list of buttons
            SelectedOrderType.IsSelected = true;
            


            EnableMask = true;
            
            if (_order._orderHeader.Customer != null)
            {
                _phone = _order._orderHeader.Customer.vCustPrimaryPh;
                _name = _order._orderHeader.Customer.vCustName;
                _address1 = _order._orderHeader.Customer.vCustAddress1;
                _address2 = _order._orderHeader.Customer.vCustAddress2;
                _city = _order._orderHeader.Customer.vCustCity;
                _state = _order._orderHeader.Customer.vCustState;
                _zipcode = _order._orderHeader.Customer.vCustZipCode;
            }
            else
                searchcustomerById(); //Display selected customer information.
            _addCustomer = AddCustomer;
        //   SelectOrderTypeCommand.Execute(SelectableOrderTypes.FirstOrDefault(n=>n.Value==order.OrderType));


           
       }

       public IList<OrderTypeModifierValue> SelectableOrderTypes
       {
           get { return _selectableOrderTypes ?? (_selectableOrderTypes = new List<OrderTypeModifierValue> { 
           
           new OrderTypeModifierValue{Value=OrderTypes.DineIn,IsSelected=boolDineIn},
           new OrderTypeModifierValue{Value=OrderTypes.ToGo,IsSelected=boolToGo},
           new OrderTypeModifierValue{Value=OrderTypes.Delivery,IsSelected=boolDelivery},
           new OrderTypeModifierValue{Value=OrderTypes.Catering,IsSelected=boolCatering},

           
           }); }
       }




       public DelegateCommand<OrderTypeModifierValue> SelectOrderTypeCommand
       {
           get { return _selectOrderTypeCommand ?? (_selectOrderTypeCommand = new DelegateCommand<OrderTypeModifierValue>(e => {

             
               if (SelectedOrderType != e)
               {

                   if (SelectedOrderType != null)
                       SelectedOrderType.IsSelected = false;


                   SelectedOrderType = e;
                   SelectedOrderType.IsSelected = true;

               }

               OnPropertyChanged("IsAdressVisible");


           })); }
       }
        public DelegateCommand<OrderTypeModifierValue> SelectCustomerCommand
        {
            get
            {
                return _selectCustomerCommand ?? (_selectCustomerCommand = new DelegateCommand<OrderTypeModifierValue>(e => {


                   
                    searchcustomer();
                    OnPropertyChanged("Phone");


                }));
            }
        }
        public void searchcustomerById()
        {

            var customerExist = GConfig.POS_Setup.Customers.FirstOrDefault(n => n.Id == _order._orderHeader.CustomerId);
            if (customerExist != null)
            {
                Phone = customerExist.vCustPrimaryPh;
                Name = customerExist.vCustName;
                Address1 = customerExist.vCustAddress1;
                Address2 = customerExist.vCustAddress2;
                City = customerExist.vCustCity;
                State = customerExist.vCustState;
                ZipCode = customerExist.vCustZipCode;
            }
        }
        public void searchcustomer()
        {

            var customerExist = GConfig.POS_Setup.Customers.FirstOrDefault(n => n.vCustPrimaryPh == Phone);
            if (customerExist!=null)
            {
                //_phone = customerExist.vCustPrimaryPh;
                Name = customerExist.vCustName;
                Address1 = customerExist.vCustAddress1;
                Address2 = customerExist.vCustAddress2;
                City = customerExist.vCustCity;
                State = customerExist.vCustState;
                ZipCode = customerExist.vCustZipCode;
            }
        }
        public DelegateCommand<OrderTypeModifierButton> OrderTypeModifierCommand
        {
            get
            {
                return _orderTypeModifierCommand ?? (_orderTypeModifierCommand = new DelegateCommand<OrderTypeModifierButton>((e) =>
                {

                    switch (e)
                    {
                        case OrderTypeModifierButton.Cancel:
                            this.CloseCommand.Execute(null);
                            break;
                        case OrderTypeModifierButton.Done:

                            _order.OrderType = SelectedOrderType.Value;
                            //Search with phone number in POS_customer If record found update Order with customer id
                            //IF record is not found then add New customer and update the new customer id in order.
                            //
                            //As a temp fix we are storing the info in comment fields //SAA 4/30
                            //_order._orderHeader.vOrderComments2 = Name;
                            //_order._orderHeader.vOrderComments3 = Address1;
                            //_order._orderHeader.vOrderComments4 = Address2;
                            //_order._orderHeader.vOrderComments5 = Phone;
                            //_order._orderHeader.vOrderComments6 = City+" "+State+" "+ZipCode;
                            _addCustomer.Invoke(Phone, Name, Address1, Address2, City, State, ZipCode);


                            this.CloseCommand.Execute(null);


                            break;
                    }
                   

                }));
            }
        }
      
    }
}
