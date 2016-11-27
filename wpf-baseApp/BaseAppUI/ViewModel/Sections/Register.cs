using BaseAppData.Entity;
using BaseAppUI.Configuration;
using BaseAppUI.Model;
using BaseAppUI.ViewModel.Notifies;
using BaseAppUI.ViewModel.Sections.Partial;
using BaseAppUI.ViewModel.Sections.Partials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace BaseAppUI.ViewModel.Sections
{
   public class Register:NotifyProperty,ISection
    {
       

        private ObservableCollection<CategoryVM> _categories;
        private DelegateCommand<string> _masterItemSearchCommand;
        private DelegateCommand<OrderVM> _orderTypeModifierCommand;
        private DelegateCommand _customItemAddCommand;
        private DelegateCommand _restTableAddCommand;


        public ObservableCollection<CategoryVM> Categories
        {
            get {

                if (_categories == null)
                {
                    _categories= new ObservableCollection<CategoryVM>(GConfig.POS_Setup.ItemCategories.Where(n=>n.vCategoryCode!="all-items" && n.vStatus!="hidden").Select(n => new CategoryVM(n,this)));
                    CategoriesInitialized = true;
                }
                return _categories;
            }
            set { _categories = value;

            OnPropertyChanged("Categories");
            }
        }

        public bool CategoriesInitialized { get; set; }

        private OrderVM _order;

        public OrderVM Order
        {
            get { 
                
                return _order ?? (_order = new OrderVM()); }
            set { _order = value;
            OnPropertyChanged("Order");

            }
        }


        MainVM _main;

        CategoryVM FrequentCategory { get; set; }

       public Register(MainVM main)
       {
           _main = main;

          
          

           FrequentCategory = new CategoryVM(
              new POS_ItemCategory { vCategoryShortDesc = "Frequent Items" },this) 
          { CategoryType=CategoryType.Frequent, IsSelected = true };

           Categories.Insert(0, FrequentCategory);

           SelectCategoryCommand.Execute(FrequentCategory);

           UpdateFrequentCategory();

       }
       public void ManipulationBoundaryFeedbackHandler
    (object sender, ManipulationBoundaryFeedbackEventArgs e)
       {
           e.Handled = true;
       }



       public Visibility POStablemodule
        {
            get { if (BaseAppUI.Properties.Settings.Default.EnableTableNumber == "Yes")
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

       CategoryVM _selectedCategory;

       public CategoryVM SelectedCategory
       {
           get { return _selectedCategory; }
           set { _selectedCategory = value;

           OnPropertyChanged("SelectedCategory");
           }
       }

       private DelegateCommand<CategoryVM> _selectCategoryCommand;

       public DelegateCommand<CategoryVM> SelectCategoryCommand
       {
           get { return _selectCategoryCommand ?? (_selectCategoryCommand = new DelegateCommand<CategoryVM>((e) => {

               if (e == null) return;
               if (SelectedCategory != e)
               {

                   if (SelectedCategory != null)
                       SelectedCategory.IsSelected = false;


                   SelectedCategory = e;
                   SelectedCategory.IsSelected = true;

               }
           
           })); }
       }

       private IList<ItemMasterVM> _selectedItemHistory;

       public IList<ItemMasterVM> SelectedItemHistory
       {
           get { return _selectedItemHistory ?? (_selectedItemHistory = new List<ItemMasterVM>()); }
       }


       ScrollViewer _scroll;

      public void Scroll_Loaded(object sender, RoutedEventArgs e)
       {
           _scroll = sender as ScrollViewer;
       }
      

       private DelegateCommand<ItemMasterVM> _selectMasterItemCommand;

       public DelegateCommand<ItemMasterVM> SelectMasterItemCommand
       {
           get
           {
               return _selectMasterItemCommand ?? (_selectMasterItemCommand = new DelegateCommand<ItemMasterVM>((e) =>
               {



                  var existline=  Order.OrderLines.FirstOrDefault(n => n._item==e._item);

                  bool isexistitem = existline != null;
                  var newline = new OrderLineVM(e._item, Order) { ItemCreated = true, Brush = e.BorderBrush };

                  if (!isexistitem)
                      SetItemVisual(e);


                  if (e.Itemmodifier == "enabled")
                  {
                      new LineItemModifier(newline, () =>
                      {
                          Func<OrderLineVM,bool> compareComments = (n) => {
                              return n.Comments == newline.Comments;
                          };

                          var matchedline = Order.OrderLines.FirstOrDefault(n => n._item == e._item && compareComments(n));

                          if (matchedline != null)
                              matchedline.Quantity++;
                          else
                          {
                              Order.OrderLines.Add(newline);
                              Order.RefreshValues();
                              _scroll.ScrollToBottom();
                          }

                      }).Show();


                  }

                  else
                  {
                      if (!isexistitem)
                      {
                          Order.OrderLines.Add(newline);
                          Order.RefreshValues();
                          _scroll.ScrollToBottom();
                      }
                      else
                          existline.Quantity++;
                  }


                



               }));
           }
       }

       BaseAppUI.Common.ColorGenerator _colorgenerator = new Common.ColorGenerator();
       public void SetItemVisual(ItemMasterVM item)
       {
        
           item.BorderBrush = _colorgenerator.GetNextBrush();
           SelectedItemHistory.Add(item);
       }


     

      


       private DelegateCommand _cancelCommand;

       public DelegateCommand CancelCommand
       {
           get { return _cancelCommand ?? (_cancelCommand = new DelegateCommand(() => {

               if (Order.Id != 0 && GConfig.ShouldKitchenCancel == false)
               {
                   Order.ResetUnSavedChanges();

                   //var e = Order;
                   //Order = null;
                   //Order = e;
               }
               

               RemoveSelection();
               Order = null;
               
               GConfig.ShouldKitchenCancel = true;

           
           })); }
           
       }


       public void RemoveSelection()
       {
           RemoveVisuals();
           UpdateFrequentCategory();
           SelectedItemHistory.Clear();
           SelectCategoryCommand.Execute(FrequentCategory);

       }

       public void RemoveSelection(int itemId)
       {
           if (!Order.OrderLines.Any(n => n._item.Id == itemId))
           {
               var mitem = SelectedItemHistory.FirstOrDefault(n => n._item.Id == itemId);
               if (mitem != null)
                   mitem.BorderBrush = null;
           }
           
       }

       private void RemoveVisuals()
       {

           foreach (var item in SelectedItemHistory)
               item.BorderBrush = null;

           _colorgenerator.Reset();
       }

       private async void UpdateFrequentCategory()
       {
            if (BaseAppUI.Properties.Settings.Default.EnableFrequentItemUpdate=="Yes")
            { 
                await Task.Factory.StartNew(() =>
                {

                    List<int> topitemids = null;
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        topitemids = GConfig.POS_Setup.OrderHeaders
                             .SelectMany(n => n.POS_OrderDetails)
                             .Where(n => n.POS_ItemMaster.vItemStatus == "publish")
                             .GroupBy(n => n.POS_ItemMasterId)
                             .OrderByDescending(n => n.Sum(k => k.fOrderQty))
                             .Take(20)
                             .Select(t => t.Key)
                             .ToList();

                    })).Wait();




                    var masteritems = Categories.Where(n => n.CategoryType == CategoryType.None)
               .SelectMany(m => m.Items).Where(m => topitemids.Contains(m._item.Id)).Select(m => m).ToList();


                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CategoryVM.FrequentItems.Clear();
                        foreach (var item in frequentSortItems(topitemids, masteritems))
                            CategoryVM.FrequentItems.Add(item);
                    }));
                });
            }
          
       }

       public IEnumerable<ItemMasterVM> frequentSortItems(IList<int> topitemids, IList<ItemMasterVM> mitems)
       {
           foreach (var i in topitemids)
           {
              var item= mitems.FirstOrDefault(n => n._item.Id == i);

              if (item != null)
                  yield return item;
           }

           yield break;
       }

        public DelegateCommand<OrderVM> KitchenPrintCommand
        {
            get
            {
                return MainVM.Main.KitchenPrintCommand;
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

       CategoryVM _searchCategory;

       public CategoryVM SearchCategory
       {
           get { return _searchCategory ?? (_searchCategory = new CategoryVM(null, this) {CategoryType=CategoryType.Search }); }
       }
       public DelegateCommand<string> MasterItemSearchCommand
       {
           get { return _masterItemSearchCommand ?? (_masterItemSearchCommand = new DelegateCommand<string>(e => {

              

               
               CategoryVM.SearchItems.Clear();

               string searchedtext = e.ToLowerInvariant();

               var results = Categories.Where(n => n.CategoryType==CategoryType.None).SelectMany(n => n.Items.Where(m => m._item.vItemDesc1.ToLowerInvariant().Contains(searchedtext)).Select(m => m));

              

               foreach (var s in results)
               {
                   CategoryVM.SearchItems.Add(s);

               }
             


              


               SelectCategoryCommand.Execute(SearchCategory);
               
              
           
           })); }
       }




       public SectionType PreviousSection { get; set; }


       public SectionType SectionType
       {
           get { return Model.SectionType.Register; }
       }


       public DelegateCommand<OrderVM> OrderTypeModifierCommand
       {
           get { return _orderTypeModifierCommand ?? (_orderTypeModifierCommand = new DelegateCommand<OrderVM>(e => {


            new Notifies.OrderTypeModifier(e,AddCustomer).Show();

           
           })); }
       }

       public DelegateCommand CustomItemAddCommand
       {
           get { return _customItemAddCommand ?? (_customItemAddCommand = new DelegateCommand(() => {


               new Notifies.CustomItemModifier(AddCustomItem).Show();
              
           
           })); }
           
       }

        
        public DelegateCommand TableAddCommand
        {
            get
            {
                return _restTableAddCommand ?? (_restTableAddCommand = new DelegateCommand(() => {

                    new Notifies.RestTableModifier(AddTableNumber).Show();
                    


                }));
            }

        }


        private void AddCustomItem(decimal price, int quantity, string customitemname)
        {

            var cat = GConfig.POS_Setup.ItemCategories.FirstOrDefault(n => n.vCategoryCode == "custom-cat");
            var catAll = GConfig.POS_Setup.ItemCategories.FirstOrDefault(n => n.vCategoryCode != "custom-cat");

            if (cat == null)
            {
                DateTime date = DateTime.Now;

                var item = new POS_ItemMaster() { vItemDesc1 = "Custom Item", dStartDate = date, dEndDate = date }; //added var to give editable custom item name SAA.
                cat = new POS_ItemCategory
                {
                    vCategoryCode = "custom-cat",
                    vStatus = "hidden",
                    vCategoryShortDesc = "Custom",
                    dEndDt = date,
                    dStartDt = date,
                    POS_ItemMasters = new List<POS_ItemMaster> { item }
                };


                GConfig.POS_Setup.ItemCategories.Add(cat);
                GConfig.DAL.SaveChanges();
            }



            var customitem = cat.POS_ItemMasters.FirstOrDefault(); //Get Normal Custom Item

            //Below code is for custom items can take user defined item description. //Added 5/6/16 //
            if (customitemname != null && customitemname.Trim() != "")
            {
                var customitemSpecial = cat.POS_ItemMasters.Where(n => n.vItemDesc1 == customitemname); //Check if custom name entered is in itemMaster

                if (customitemSpecial.Count() == 0 && customitem.vItemDesc1 != customitemname)
                {
                    Int32 customitemid = cat.POS_ItemMasters.Max(p => p.itemID); //get max itemid of custom items.
                    Int32 ItemMasterid = catAll.POS_ItemMasters.Max(p => p.Id); //get max of id from itemMaster.

                    ItemMasterid++;
                    if (customitemid == 0)
                        customitemid = 9000;
                    else
                        customitemid++;
                    if (customitem.vItemDesc1 != customitemname)
                    {
                        DateTime date = DateTime.Now;

                        var item = new POS_ItemMaster() { Id = ItemMasterid, itemID = customitemid,vItemStatus="hidden", vItemDesc1 = customitemname, dStartDate = date, dEndDate = date }; //added var to give editable custom item name SAA.
                        cat.POS_ItemMasters.Add(item);
                        GConfig.DAL.SaveChanges();
                        customitem = item;
                    }
                }
                if (customitemSpecial != null)
                    customitem = customitemSpecial.FirstOrDefault();
            }

            //Above code is for custom items can take user defined item description. //Added 5/6/16
            if (customitem != null)
            {
                OrderLineVM line = new OrderLineVM(customitem, Order, new POS_OrderDetails()
                {
                    POS_OrderHeaderId = Order._orderHeader.Id,
                    POS_ItemMasterId = customitem.Id,

                    fLineItemPrice = price,
                    fOrderQty = quantity,
                    fLineSubTotal = price * quantity
                })
                { ItemCreated = true };



                Order.OrderLines.Add(line);

                Order.RefreshValues();

                _scroll.ScrollToBottom();




            }

        }

        

        private void AddCustomer(string customerPhone,string customerName,string customerAddr1,string customerAddr2,string customerCity,string customerState,string customerZipcode)
        {

            Int32 _iCustID = GConfig.POS_Setup.Customers.Max(n => n.iCustid);
            Int32 _custRecordid = GConfig.POS_Setup.Customers.Max(n => n.Id);
            var customerExist = GConfig.POS_Setup.Customers.FirstOrDefault(n => n.vCustPrimaryPh == customerPhone);

            if (customerExist == null)
            {
                DateTime date = DateTime.Now;
                _iCustID++;
                _custRecordid++;

                var customer = new POS_Customer()
                {
                    iCustid = _iCustID,
                    vCustPrimaryPh = customerPhone,
                    vCustName = customerName,
                    vCustAddress1 = customerAddr1,
                    vCustAddress2 = customerAddr2,
                    vCustCity = customerCity,
                    vCustState = customerState,
                    vCustZipCode = customerZipcode,
                    vCustStatus = "Publish"
                }; //added var to give editable custom item name SAA.
                GConfig.POS_Setup.Customers.Add(customer);
                GConfig.DAL.SaveChanges();
                Order._orderHeader.CustomerId = _custRecordid;

            }
            else
            {



                customerExist.vCustName = customerName;
                customerExist.vCustAddress1 = customerAddr1;
                customerExist.vCustAddress2 = customerAddr2;
                customerExist.vCustCity = customerCity;
                customerExist.vCustState = customerState;
                customerExist.vCustZipCode = customerZipcode;
                customerExist.vCustStatus = "Publish";
               
                GConfig.DAL.SaveChanges();
               
                Order._orderHeader.CustomerId = customerExist.Id;
            }
        }



        private void AddTableNumber(string tableNumber)
        {

            //Int32 _iCustID = GConfig.POS_Setup.Customers.Max(n => n.iCustid);
            //Int32 _custRecordid = GConfig.POS_Setup.Customers.Max(n => n.Id);
            //var customerExist = GConfig.POS_Setup.Customers.FirstOrDefault(n => n.vCustPrimaryPh == customerPhone);
            _order._orderHeader.vOrderTableNumber = tableNumber;
            Order._orderHeader.vOrderTableNumber = tableNumber;
            Order.TableNumber = tableNumber;
            
            
           
        }
    }
}
