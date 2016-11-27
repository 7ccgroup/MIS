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
using BaseAppUI.View.Notifies;
using System.ComponentModel;
using System.Windows.Controls;

namespace BaseAppUI.ViewModel.Sections
{
    public class Items : NotifyProperty, ISection
    {


        MainVM _main;
        ItemCategoryListForDisplay _selectedCategory;
        
        public Items(MainVM main)
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

        

        //private Window ItemEditWindow = new ItemEntryView();


        //private DelegateCommand _selectItemEditCommand;

        //public DelegateCommand SelectItemEditCommand
        //{
        //    get
        //    {
        //        return _selectItemEditCommand ?? (_selectItemEditCommand = new DelegateCommand(() => {


        //            //      var win = new ItemEntryView();
        //            if (ItemEditWindow != null)
        //            {
        //                ItemEditWindow.ShowDialog();
                        
        //            }
        //            else
        //            {
        //                ItemEditWindow = new ItemEntryView();
        //                ItemEditWindow.ShowDialog();
        //            }
        //            // new ItemEdit(e).Show(null);


        //        }));
        //    }

        //}
        

        private ItemsBuilder _itemBuilder = new ItemsBuilder();
        ////////////////Items Command......
        private IList<ItemListForDisplay> _items;
        public IList<ItemListForDisplay> ItemsDisplay
        {
            get
            {
                _items = _itemBuilder.ItemListDisplayAdvance();
                return _items;
            }
        }

        public IList<ItemListForDisplay> ItemsDisplayByCategory
        {
            get
            {
                if (_selectedCategory != null)
                    _items = _itemBuilder.ItemListDisplayAdvance(_selectedCategory.vCategoryShortDesc);
                else
                    _items = _itemBuilder.ItemListDisplayAdvance("ALLITEMS");
                
                return _items;

            }
           // set { OnPropertyChanged("SelectedCategory"); }
        }

        private IList<ItemCategoryListForDisplay> _itemscategory;
        public IList<ItemCategoryListForDisplay> ItemCategory
        {
            get
            {


                _itemscategory = _itemBuilder.ItemCategoryListDisplay();
                
                return _itemscategory;

            }
        }

        /// <summary>
        /// This is his for properties taken from Register View Model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        
        public long ItemNumber
        {
            get {if (_selectMasterItem != null)
                    return _selectMasterItem.itemID;
                else
                    return 0;
            }
            
            
        }

       
        public Visibility EnableItemAdd
        {
            get
            {
                if (BaseAppUI.Properties.Settings.Default.EnableItemAdd=="Yes")
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }


        }
        //This property will enable "Edit" Button if Item is selected.
        public bool RecordSelected
        {
            get { if (_selectMasterItem != null)
                    return true;
                else
                    return false; }

        }

        private ItemListForDisplay _selectMasterItem;
        public ItemListForDisplay SelectMasterItem
        {
            get { return _selectMasterItem; }
            set { _selectMasterItem = value;
                //    _main.ItemEntry.SelctedItemForEdit = value;
                SelectMasterItem.IsSelected = true;
               OnPropertyChanged("SelectMasterItem");
                OnPropertyChanged("RecordSelected");
            }
        }
        
         private DelegateCommand<ItemListForDisplay> _selectMasterItemCommand;

        public DelegateCommand<ItemListForDisplay> SelectMasterItemCommand
        {
            get
            {
                return _selectMasterItemCommand ?? (_selectMasterItemCommand = new DelegateCommand<ItemListForDisplay>((e) =>
                {


                    SelectMasterItem = e; //select the item on click from screen SAA.
                    SelectMasterItem.IsSelected = true;
                    
                  //  OnPropertyChanged("SelectMasterItem101");
                  //  MessageBox.Show(_selectMasterItem.itemID.ToString());
                   

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
                    _categories = _itemBuilder.ItemCategoriesListDisplay();
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
            get { return Model.SectionType.Items; }
        }


        private DelegateCommand<ItemListForDisplay> _selectLineItemCommand;

        public DelegateCommand<ItemListForDisplay> SelectLineItemCommand
        {
            get
            {
                return _selectLineItemCommand ?? (_selectLineItemCommand = new DelegateCommand<ItemListForDisplay>((e) => {

                    e.ActionMode = "Item Edit";
                    new ItemEdit(e).Show(null);

                //    new ItemEntryView(e).Show(null);


                }));
            }

        }
        private DelegateCommand<ItemListForDisplay> _selectLineItemAddCommand;

        public DelegateCommand<ItemListForDisplay> SelectLineItemAddCommand
        {
            get
            {
                return _selectLineItemAddCommand ?? (_selectLineItemAddCommand = new DelegateCommand<ItemListForDisplay>((e) => {
                    e.ActionMode = "Item Add";
                    e.itemID = 0;
                    new ItemEdit(e).Show(null);



                }));
            }

        }

    }
    public class ItemEntry:NotifyProperty //ViewModel for ItemEntry
    {
        private readonly IChildWindow _childWindow;
        private readonly IChildWindowNested _childWindowNested;
        private ICommand _okCommand;
        private ICommand _openCommand;


        MainVM _main;
        string _SourceOfCall;
        //public ItemEntry(IChildWindow childWindow, IChildWindowNested childWindowNested)
        //{
        //    _childWindow = childWindow;
        //    _childWindowNested = childWindowNested;
        //   // _main = new MainVM(); //main Vm is passed to all classes.
        //}
        public ItemEntry()
        {
            _SourceOfCall = "View";
        }
        public ItemEntry(MainVM main)
        {
            _main = main;
        }
       
        public ICommand OpenCommand
        {
            get
            {
                return _openCommand ?? (_openCommand = new DelegateCommand(OpenClick));
            }
        }
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new DelegateCommand(OkClick));
            }
        }

        private void OpenClick()
        {
            _childWindowNested.SetOwner(_childWindow);
            _childWindowNested.ShowDialog();
        }

        private void OkClick()
        {
            _childWindow.DialogResult = true;
            _childWindow.Close();
        }
        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (_isOpen == value)
                    return;
                _isOpen = value;
                OnPropertyChanged("IsOpen");
            }
        }

        private ItemListForDisplay _SelectedItemForEdit;
        public ItemListForDisplay SelctedItemForEdit
        {
            get {return _SelectedItemForEdit; }
            set
            {
                _SelectedItemForEdit = _main.Items.SelectMasterItem;
                OnPropertyChanged("SelectMasterItem");
            }
        }

        private DelegateCommand _cancelItemEditCommand;

        public DelegateCommand CancelItemEditCommand
        {

            get
            {
                return _cancelItemEditCommand ?? (_cancelItemEditCommand = new DelegateCommand(() =>
                {


                    //ItemEditWindow.Close();
                    // this.close();
                    //Application.Current.MainWindow.Close();
                    foreach (System.Windows.Window win in System.Windows.Application.Current.Windows)
                    {
                        if (win.DataContext == this)
                            win.Visibility = Visibility.Hidden;
                    }
                    MessageBox.Show("this is close,,,,,,,,,from ItemEntry,,,,,");

                }));
            }

        }

    }


public interface IChildWindow  //Constructor Injection This is needed for each Child window.
    {
        void Close();
        bool? ShowDialog();
        void SetOwner(Object window);
        bool? DialogResult { get; set; }
    }

    public interface IChildWindowNested
    {
        void Close();
        bool? ShowDialog();
        void SetOwner(Object window);
        bool? DialogResult { get; set; }
    }


public static class CustomDesignAttributes
    {
        private static bool? _isInDesignMode;

        public static DependencyProperty VerticalScrollToProperty = DependencyProperty.RegisterAttached(
          "VerticalScrollTo",
          typeof(double),
          typeof(CustomDesignAttributes),
          new PropertyMetadata(ScrollToChanged));

        public static DependencyProperty HorizontalScrollToProperty = DependencyProperty.RegisterAttached(
          "HorizontalScrollTo",
          typeof(double),
          typeof(CustomDesignAttributes),
          new PropertyMetadata(ScrollToChanged));

        private static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode =
                      (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                }

                return _isInDesignMode.Value;
            }
        }

        public static void SetVerticalScrollTo(UIElement element, double value)
        {
            element.SetValue(VerticalScrollToProperty, value);
        }

        public static double GetVerticalScrollTo(UIElement element)
        {
            return (double)element.GetValue(VerticalScrollToProperty);
        }

        public static void SetHorizontalScrollTo(UIElement element, double value)
        {
            element.SetValue(HorizontalScrollToProperty, value);
        }

        public static double GetHorizontalTo(UIElement element)
        {
            return (double)element.GetValue(HorizontalScrollToProperty);
        }

        private static void ScrollToChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!IsInDesignMode)
                return;
            ScrollViewer viewer = d as ScrollViewer;
            if (viewer == null)
                return;
            if (e.Property == VerticalScrollToProperty)
            {
                viewer.ScrollToVerticalOffset((double)e.NewValue);
            }
            else if (e.Property == HorizontalScrollToProperty)
            {
                viewer.ScrollToHorizontalOffset((double)e.NewValue);
            }
        }
    }

}
