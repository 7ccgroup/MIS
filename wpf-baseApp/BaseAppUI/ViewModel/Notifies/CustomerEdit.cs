using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace BaseAppUI.ViewModel.Notifies
{
    public enum CustomerEditButton
    {
        Addition,
        Subtract,
        Delete,
        Done,
        Save,
        Cancel
    }
    public class CustomerEdit : NotifyBase //This class is used to edit line item on receipt in Register Screen SAA.
    {
        CustomerListForDisplay _lineitem;
        public CustomerEdit(CustomerListForDisplay lineitem)
        {
            _lineitem = lineitem;
            SelectedEditCustomer = lineitem;
            IsPopup = true;
            EnableMask = true;

            // _quantity = _lineitem.;
        }

        public string CustomerName
        {
            get { return _lineitem.vCustName; }
        }


        //below code is to modify quantity of lineItem.
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

        //public decimal Price
        //{
        //    get { return convert.decimal(_lineitem.vItemPrice); }
        //}

        //public decimal Subtotal
        //{
        //    get { return (decimal)Price * Quantity; }
        //}

        private CustomerListForDisplay _SelectedEditCustomer;
        public CustomerListForDisplay SelectedEditCustomer
        {
            get { return _SelectedEditCustomer; }
            set { _SelectedEditCustomer = _lineitem; }
        }

        private DelegateCommand<CustomerEditButton> _CustomerEditCommand;

        public DelegateCommand<CustomerEditButton> CustomerEditCommand
        {
            get
            {
                return _CustomerEditCommand ?? (_CustomerEditCommand = new DelegateCommand<CustomerEditButton>((e) => {

                    switch (e)
                    {

                        case CustomerEditButton.Addition:
                            Quantity++;
                            break;
                        case CustomerEditButton.Subtract:
                            if (Quantity < 2) return;
                            else Quantity--;
                            break;
                        case CustomerEditButton.Cancel:

                        //    var ordervm = _lineitem._parent;
                        //    ordervm.OrderLines.Remove(_lineitem);
                        //    if (_lineitem._line.Id != 0)
                        //        GConfig.DAL.StateChange(_lineitem._line, System.Data.Entity.EntityState.Deleted);
                        //    ordervm.RefreshValues();
                            this.CloseCommand.Execute(null);


                            break;
                        case CustomerEditButton.Save:
                            //        _lineitem.Quantity = Quantity;
                         //   MessageBox.Show("value of Cut name" + _lineitem.vCustName);
                            CustomersBuilder _customerBuilder = new CustomersBuilder();
                            _customerBuilder.CustomerUpdate(SelectedEditCustomer);
                            this.CloseCommand.Execute(null);
                            break;


                        default: break;
                    }

                }));
            }
        }
    }
}
