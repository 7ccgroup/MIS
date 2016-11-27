using BaseAppData.Entity;
using BaseAppUI.Configuration;
using BaseAppUI.ViewModel.Sections.Partial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BaseAppUI.ViewModel.Sections.Partials
{

    public enum CategoryType
    {
        None,
        Frequent,
        Search,

    }
   public class CategoryVM:NotifyProperty
    {

       POS_ItemCategory _category;
       Register _parent;
       public CategoryType CategoryType { get; set; }
      

       public CategoryVM(POS_ItemCategory category,Register parent)
       {
           _parent = parent;
           _category = category;
       }

       private bool _isSelected;

       public bool IsSelected
       {
           get { return _isSelected; }
           set { _isSelected = value;
           OnPropertyChanged("IsSelected");
           }
       }

       public string Name
       {
           get { return _category.vCategoryShortDesc.ToUpperInvariant(); }
       }
       public int CatID
       {
           get { return _category.CatID; }
       }

       private static ObservableCollection<ItemMasterVM> _frequentItems;

       public static ObservableCollection<ItemMasterVM> FrequentItems
       {
           get { return CategoryVM._frequentItems ?? (_frequentItems = new ObservableCollection<ItemMasterVM>()); }
           set
           {
               _frequentItems = value;
           }
       }
       private static ObservableCollection<ItemMasterVM> _searchItems;

       public static ObservableCollection<ItemMasterVM> SearchItems
       {
           get { return CategoryVM._searchItems ?? (_searchItems = new ObservableCollection<ItemMasterVM>()); }
           set { CategoryVM._searchItems = value; }
       }



       public bool ItemsInitialized { get; set; }

        private ICollection<ItemMasterVM> _items;

       public ICollection<ItemMasterVM> Items
       {

           get {


              
               if(CategoryType==CategoryType.None ){

                   if (_items == null)
                   {
                       if (_category.POS_ItemMasters.Any())
                           _items = new ObservableCollection<ItemMasterVM>(_category.POS_ItemMasters.Select(n => new ItemMasterVM(n)));
                       else
                       _items = new ObservableCollection<ItemMasterVM>();
                       ItemsInitialized = true;
                   }

                 

                    return _items;
               }

               if (CategoryType==CategoryType.Frequent)
                   return FrequentItems;
               else
                   return SearchItems;

              
             
           }
       }

       ICollectionView _masterItems;

       public ICollectionView MasterItems
       {
           get {

               if (_masterItems == null)
                   _masterItems = CollectionViewSource.GetDefaultView(Items);

               
               return _masterItems; }
       }
       public DelegateCommand<ItemMasterVM> SelectMasterItemCommand
       {
           get
           {
               return _parent.SelectMasterItemCommand;
           }
       }


       
  

    }
}
