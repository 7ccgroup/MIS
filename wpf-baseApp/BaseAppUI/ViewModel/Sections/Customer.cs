using BaseAppData.Entity;
using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.ViewModel.Sections.Partial;
using BaseAppUI.ViewModel.Sections.Partials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data;
using BaseAppUI.Common;
using BaseAppUI.ViewModel.Notifies;

namespace BaseAppUI.ViewModel.Sections
{
    public class Customers : NotifyProperty, ISection
    {


        MainVM _main;
        ItemCategoryListForDisplay _selectedCategory;

        public Customers(MainVM main)
        {

            _main = main;

            //  _main.SwitchToView(Model.SectionType.Report);


        }

        DelegateCommand _closeWindowCommand;

        public DelegateCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ?? (_closeWindowCommand = new DelegateCommand(() =>
                {

                    Application.Current.Shutdown();

                }));
            }
        }



        private DelegateCommand<SectionType> _switchToSectionCommand;

        public DelegateCommand<SectionType> SwitchToSectionCommand
        {
            get
            {
                return _switchToSectionCommand ?? (_switchToSectionCommand = new DelegateCommand<SectionType>((e) =>
                {

                    _main.SwitchToView(e);


                }));
            }
        }

        private DelegateCommand _itemsCommand;

        public DelegateCommand ItemsCommand
        {
            get
            {
                return _itemsCommand ?? (_itemsCommand = new DelegateCommand(() =>
                {

                    MessageBox.Show("Hello this is good start, have to complete this IA");
                    //   _main.SwitchToView(SectionType.Dashboard);

                }));
            }

        }

        private DelegateCommand _returnCommand;

        public DelegateCommand ReturnCommand
        {
            get
            {
                return _returnCommand ?? (_returnCommand = new DelegateCommand(() =>
                {

                    _main.SwitchToView(SectionType.Dashboard);

                }));
            }

        }




        private CustomersBuilder _customerBuilder = new CustomersBuilder();
        ////////////////Customers Command......
        private IList<CustomerListForDisplay> _customers;
        public IList<CustomerListForDisplay> CustomersDisplay
        {
            get
            {
                _customers = _customerBuilder.CustomerListDisplayAdvance("");
                return _customers;
            }
        }

        public IList<CustomerListForDisplay> CustomersDisplayByCategory
        {
            get
            {
                if (_selectedCategory != null)
                    _customers = _customerBuilder.CustomerListDisplayAdvance(_selectedCategory.vCategoryShortDesc);
                else
                    _customers = _customerBuilder.CustomerListDisplayAdvance("ALLITEMS");
                
                return _customers;

            }
            
        }

        private IList<ItemCategoryListForDisplay> _itemscategory;
        public IList<ItemCategoryListForDisplay> ItemCategory
        {
            get
            {


                _itemscategory = _customerBuilder.ItemCategoryListDisplay();

                return _itemscategory;

            }
        }

        /// <summary>
        /// This is his for properties taken from Register View Model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private CustomerListForDisplay _selectMasterCustomer;
        public CustomerListForDisplay SelectMasterCustomer
        {
            get { return _selectMasterCustomer; }
            set
            {
                _selectMasterCustomer = value;
                CustomerNumber = _selectMasterCustomer.Id.ToString();
                OnPropertyChanged("SelectMasterCustomer");
                OnPropertyChanged("RecordSelected");
            }
        }

        public bool RecordSelected
        {
            get
            {
                if (_selectMasterCustomer != null)
                    return true;
                else
                    return false;
            }

        }
        private DelegateCommand<CustomerListForDisplay> _selectMasterCustomerCommand;

        public DelegateCommand<CustomerListForDisplay> SelectMasterCustomerCommand
        {
            get
            {
                return _selectMasterCustomerCommand ?? (_selectMasterCustomerCommand = new DelegateCommand<CustomerListForDisplay>((e) =>
                {


                    SelectMasterCustomer = e; //select the item on click from screen SAA.
                    SelectMasterCustomer.IsSelected = true;

                }));
            }
        }

        private ObservableCollection<ItemCategoryListForDisplay> _categories;
        private DelegateCommand<string> _masterItemSearchCommand;



        public ObservableCollection<ItemCategoryListForDisplay> CategoriesList
        {
            get
            {

                if (_categories == null)
                {
                    _categories = _customerBuilder.ItemCategoriesListDisplay();
                    //new ObservableCollection<CategoryVM>(GConfig.POS_Setup.ItemCategories.Where(n => n.vCategoryCode != "all-items").Select(n => new CategoryVM(n, this)));
                    CategoriesInitialized = true;
                }
                return _categories;
            }
            set
            {
                _categories = value;

                OnPropertyChanged("Categories");
            }
        }

        public bool CategoriesInitialized { get; set; }


        public ItemCategoryListForDisplay SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;

                OnPropertyChanged("ItemsDisplayByCategory"); //saa changed to ItemsDisplayByCategory it was SelectedCategory
            }
        }

        private DelegateCommand<ItemCategoryListForDisplay> _selectCategoryCommand;

        public DelegateCommand<ItemCategoryListForDisplay> SelectCategoryCommand
        {
            get
            {
                return _selectCategoryCommand ?? (_selectCategoryCommand = new DelegateCommand<ItemCategoryListForDisplay>((e) => {

                    if (e == null) return;
                    if (SelectedCategory != e)
                    {

                        if (SelectedCategory != null)
                            SelectedCategory.IsSelected = false;


                        SelectedCategory = e;
                        SelectedCategory.IsSelected = true;

                    }

                }));
            }
        }

        ////This is for Customer Edit

        private DelegateCommand<CustomerListForDisplay> _selectLineCustomerCommand;

        public DelegateCommand<CustomerListForDisplay> SelectLineCustomerCommand
        {
            get
            {
                return _selectLineCustomerCommand ?? (_selectLineCustomerCommand = new DelegateCommand<CustomerListForDisplay>((e) => {

                    new CustomerEdit(e).Show(null);

                    //    new ItemEntryView(e).Show(null);


                }));
            }

        }
        private string _customerNumber;
        public string CustomerNumber
        {
            get {
                return _customerNumber;
            }
            set { _customerNumber = value;
                OnPropertyChanged("CustomerNumber");
            }
        }
        /// <summary>
        /// This is his for properties taken from Register View Model
        public void ManipulationBoundaryFeedbackHandler
  (object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
        public SectionType PreviousSection { get; set; }
        public SectionType SectionType
        {
            get { return Model.SectionType.Customers; }
        }
    }
}
