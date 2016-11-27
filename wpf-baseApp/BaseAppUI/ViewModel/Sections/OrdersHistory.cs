using BaseAppData.Entity;
using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace BaseAppUI.ViewModel.Sections
{
    public class OrdersHistory : NotifyProperty, ISection
    {

        public OrdersHistory()
        {

            SelectOrderTypesCommand.Execute(AllTypes[0]);
        }

        private IList<OrderTypeVM> _allTypes;
        public IList<OrderTypeVM> AllTypes
        {
            get
            {
                return _allTypes ?? (_allTypes = new List<OrderTypeVM> {

                 new OrderTypeVM("ALL ORDERS",OrderTypes.None,this),
                  new OrderTypeVM("ONLINE", OrderTypes.Online, this),
                  new OrderTypeVM("DINE-IN", OrderTypes.DineIn, this),
                  new OrderTypeVM("TO-GO", OrderTypes.ToGo, this),
                 new OrderTypeVM("DELIVERY", OrderTypes.Delivery, this),
                 new OrderTypeVM("CATERING", OrderTypes.Catering, this)
               });
            }

        }
        public void ManipulationBoundaryFeedbackHandler
   (object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }



        private OrderTypeVM _selectedOrderType;

        public OrderTypeVM SelectedOrderType
        {
            get { return _selectedOrderType; }
            set {
                if (value != _selectedOrderType)
                {
                    _selectedOrderType = value;
                    OnPropertyChanged("Orders");

                }

            }
        }

        private OrderVM _selectedOrder;

        public OrderVM SelectedOrder
        {
            get { return _selectedOrder; }
            set { _selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }



        private DelegateCommand<OrderTypeVM> _selectOrderTypesCommand;

        public DelegateCommand<OrderTypeVM> SelectOrderTypesCommand
        {
            get { return _selectOrderTypesCommand ?? (_selectOrderTypesCommand = new DelegateCommand<OrderTypeVM>((e) => {


                if (e == null) return;
                if (SelectedOrderType != e)
                {

                    if (SelectedOrderType != null)
                        SelectedOrderType.IsSelected = false;


                    SelectedOrderType = e;
                    SelectedOrderType.IsSelected = true;

                    selectorders(SelectedOrderType._ordertype);

                }


            })); }
        }

        private DelegateCommand<OrderVM> _selectOrderCommand;

        public DelegateCommand<OrderVM> SelectOrderCommand
        {
            get
            {
                return _selectOrderCommand ?? (_selectOrderCommand = new DelegateCommand<OrderVM>((e) =>
                {


                    if (e == null) return;
                    if (SelectedOrder != e)
                    {

                        if (SelectedOrder != null)
                            SelectedOrder.IsSelected = false;


                        SelectedOrder = e;
                        SelectedOrder.IsSelected = true;

                    }


                }));
            }
        }


        private ObservableCollection<OrderVM> _orders;

        public ObservableCollection<OrderVM> Orders
        {
            get { return _orders ?? (_orders = new ObservableCollection<OrderVM>()); }
        }

        private async void selectorders(string type)
        {

            await Task.Run(() => {

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Orders.Clear();
                })).Wait();

                IEnumerable<POS_OrderHeader> orders = null;

                if (type == OrderTypes.None)
                    orders = GConfig.POS_Setup.OrderHeaders.OrderByDescending(n => n.dModifiedDate).Select(n => n);
                else
                    orders = GConfig.POS_Setup.OrderHeaders.Where(n => n.vOrderType == type).OrderByDescending(n => n.dModifiedDate).Select(n => n);

                foreach (var order in orders)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => {

                        Orders.Add(new OrderVM(order));
                    })).Wait();

                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {

                    if (SelectedOrder != null)
                    {
                        foreach (var n in _orders)
                        {
                            if (n._orderHeader == SelectedOrder._orderHeader)
                            {
                                SelectOrderCommand.Execute(n);
                                break;
                            }

                        }
                    }
                }));


            });

        }

        public DelegateCommand ReturnCommand
        {
            get
            {
                return MainVM.Main.ReturnCommand;
            }

        }
        

        private string _orderHistoryDateSelected = System.DateTime.Today.ToString();
        public string OrderHistoryDateSelected
        {
            get { return _orderHistoryDateSelected; }
            set
            {
                _orderHistoryDateSelected = value;
                selectordersByStatus(_orderStatusSelected,_orderHistoryDateSelected);
                OnPropertyChanged("Orders");
            }
        }

        private string _orderStatusSelected="Select";
        public string OrderStatusSelected
        {
            get { return _orderStatusSelected; }
            set { _orderStatusSelected = value;
                selectordersByStatus(_orderStatusSelected,_orderHistoryDateSelected);
                OnPropertyChanged("Orders");
            }
        }


      
        
        
        public List<string> OrderStatusListSearch
        {
           get {
             
               List<string> StatusList = new List<string>();

                StatusList.Add("Select");
               StatusList.Add("Pending");
                StatusList.Add("Completed");
                StatusList.Add("Refunded");
                return StatusList;
                } 
            
          
        }

        private DelegateCommand _selectordersByStatusCommand;
        public DelegateCommand selectordersByStatusCommand
        {
            get
            {
                return _selectordersByStatusCommand ?? (_selectordersByStatusCommand = new DelegateCommand(() =>
                {


                   

                        selectordersByStatus(_orderStatusSelected, _orderHistoryDateSelected);

                    


                }));
            }
        }
        private async void selectordersByStatus(string Statustype,string OrderSearchDate)
        {
            DateTime _orderSearchDate;
            _orderSearchDate = Convert.ToDateTime(OrderSearchDate);

            await Task.Run(() => {

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Orders.Clear();
                })).Wait();

                IEnumerable<POS_OrderHeader> orders = null;

                if (Statustype == "Select")
                    orders = GConfig.POS_Setup.OrderHeaders.OrderByDescending(n => n.dModifiedDate).Select(n => n);
                else
                    orders = GConfig.POS_Setup.OrderHeaders.Where( n => n.vOrderStatus == Statustype && n.dCreatedDate.Year == _orderSearchDate.Year
                            && n.dCreatedDate.Month == _orderSearchDate.Month && n.dCreatedDate.Day == _orderSearchDate.Day).OrderByDescending(n => n.dModifiedDate).Select(n => n);

                foreach (var order in orders)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => {

                        Orders.Add(new OrderVM(order));
                    })).Wait();

                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {

                    if (SelectedOrder != null)
                    {
                        foreach (var n in _orders)
                        {
                            if (n._orderHeader == SelectedOrder._orderHeader)
                            {
                                SelectOrderCommand.Execute(n);
                                break;
                            }

                        }
                    }
                }));


            });

        }


        public SectionType PreviousSection { get; set; }
       public SectionType SectionType
       {
           get { return Model.SectionType.Orders; }
       }
    }
}
