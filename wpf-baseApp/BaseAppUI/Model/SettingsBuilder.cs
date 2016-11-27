using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BaseAppData;
using BaseAppUI.Common;
using System.Data.SqlServerCe;
using System.Data.Odbc;
using System.Configuration;
using System.Reflection;
using System.Windows.Input;
using System.Collections;
using BaseAppUI.ViewModel.Sections.Partials;
using System.Collections.ObjectModel;
using Dapper;

namespace BaseAppUI.Model
{
    public struct SettingStruct 
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        private bool _isSelected;
       
    }
    public class SettingScreen : NotifyProperty
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        private bool _isSelected;
        public SettingScreen(string label, string name, string value)
        {
            Label = label;
            Name = name;
            Value = value;
        }
    }
    public class SettingScreenEdit : NotifyProperty
    {
        public string DBstring { get; set; }
        public string POSMainPrinter { get; set; }
        public string POSKitchenPrinter { get; set; }
        public string RestaurantAddress1 { get; set; }
        public string RestaurantAddress2 { get; set; }
        public string RestaurantTel { get; set; }
        public string ServiceFee { get; set; }
        public string MerchantID { get; set; }
        public string UserID { get; set; }
        public string Tax1_State { get; set; }
        public string Tax2_Village { get; set; }
        public string Tax_overall { get; set; }
        public string Pin { get; set; }
        public string TipSignModifierSwitch { get; set; }
        public string RestaurantName { get; set; }
        public string Url { get; set; }
        public string CKey { get; set; }
        public string CSecret { get; set; }
        public string DatabaseLocation { get; set; }
        public string DashboardIncludePendingOrders { get; set; }
        public string SyncMode { get; set; }
        public string AddonCount { get; set; }
        public string Addon201 { get; set; }
        public string Addon202 { get; set; }
        public string Addon203 { get; set; }
        public string Addon204 { get; set; }
        public string Addon205 { get; set; }
        public string LoginScreen { get; set; }

        public string SettingDisplay01Label { get; set; }
        public string SettingDisplay02Label { get; set; }
        public string SettingDisplay03Label { get; set; }
        public string SettingDisplay04Label { get; set; }
        public string SettingDisplay05Label { get; set; }
        public string SettingDisplay06Label { get; set; }
        public string SettingProperty01Name { get; set; }
        public string SettingProperty02Name { get; set; }
        public string SettingProperty03Name { get; set; }
        public string SettingProperty04Name { get; set; }
        public string SettingProperty05Name { get; set; }
        public string SettingProperty06Name { get; set; }

        public string SettingDisplay01Value { get; set; }
        public string SettingDisplay02Value { get; set; }
        public string SettingDisplay03Value { get; set; }
        public string SettingDisplay04Value { get; set; }
        public string SettingDisplay05Value { get; set; }
        public string SettingDisplay06Value { get; set; }
        public string SettingGroupid { get; set; }
    }
    public class SettingsList:NotifyProperty
    {
        public int ID { get; set; }
        public int iCorpId { get; set; }

        public string vTabCode { get; set; }

        public string vFldCode { get; set; }

        public string vCodeDesc { get; set; }
 
        public string vCodeSeq { get; set; }
      
        public string vAltCode { get; set; }
      
        public string vCodePurpose { get; set; }
       
        public string vStatus { get; set; }

        public DateTime dStartDt { get; set; }
        public DateTime dEndDt { get; set; }

      
        public string vEntryBy { get; set; }

        public long? tTimestamp { get; set; }

    }
    public class SettingsListForDisplay : NotifyProperty
    {
        
        public string Name { get; set; }
  
        private bool _isSelected;

        public SettingsListForDisplay()
        {

        }
        public SettingsListForDisplay(string _name, bool _isSelected)
        {
            Name = _name;
            IsSelected = _isSelected;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

    }

    public class SettingsCategoryListForDisplay : NotifyProperty
    {
        public Int64 Id { get; set; }
        public Int64 CatID { get; set; }
        public string vCategoryCode { get; set; }
        public string vCategoryDesc { get; set; }
        public string vCategoryShortDesc { get; set; }
        public string vComments { get; set; }
        public string vStatus { get; set; }

        public string dStartDate { get; set; }
        public string dEndDate { get; set; }
        public string vEntryBy { get; set; }
        public string tTimestamp { get; set; }


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
    }
    class SettingsBuilder : SqlCEDataAccess
    {


        public DataTable POSitems(string EntityType)
        {
            if (EntityType == "Items")
                return ItemList();
            else
                return ItemList();

        }
        public DataTable ItemList() //List of Items 
        {
            string _vItemStatus = "Publish";
            string SQL_str = "SELECT Id" +
                              ",itemID" +
                              ",vItemSku" +
                              ",vItemDesc1" +
                              ",vItemDesc2" +
                              ",vItemPrice" +
                              ",vItemRelatedID" +
                              ",vItemMinPrice" +
                              ",vItemVendor" +
                              ",vItemVendorContact" +
                              ",vItemVendorPhone" +
                              ",vItemNotes" +
                              ",vItemStatus" +
                              ",vItemAvailability" +
                              ",dStartDate" +
                              ",dEndDate" +
                              ",vEntryBy" +
                              ",tTimestamp" +
                              ",POS_ItemCategoryId" +
                          " FROM POS_ItemMaster" + " where vItemStatus='" + _vItemStatus + "'";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSitemMaster");
            ResultTable = ResultSet.Tables[0];

            return ResultTable;


        }

        //public IList<SettingsListForDisplay> SettingsListDisplay() //List of Items 
        //{
        //    string _vItemStatus = "Publish";
        //    string SQL_str = "SELECT Id" +
        //                      ",itemID" +
        //                      ",vItemSku" +
        //                      ",vItemDesc1" +
        //                      ",vItemDesc2" +
        //                      ",vItemPrice" +
        //                      ",vItemRelatedID" +
        //                      ",vItemMinPrice" +
        //                      ",vItemVendor" +
        //                      ",vItemVendorContact" +
        //                      ",vItemVendorPhone" +
        //                      ",vItemNotes" +
        //                      ",vItemStatus" +
        //                      ",vItemAvailability" +
        //                      ",dStartDate" +
        //                      ",dEndDate" +
        //                      ",vEntryBy" +
        //                      ",tTimestamp" +
        //                      ",POS_ItemCategoryId" +
        //                  " FROM POS_ItemMaster" + " where vItemStatus='" + _vItemStatus + "'";


        //    //Connect to database and retrieve
        //    SettingsListForDisplay Obj = new SettingsListForDisplay();
        //    IList<SettingsListForDisplay> ALLitems;
        //    SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
        //    ALLitems = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);

        //    return ALLitems;


        //}
        public IList<SettingsCategoryListForDisplay> SettingsCategoryListDisplay() //List of Items 
        {
            string _vStatus = "Publish";
            string SQL_str = "SELECT Id" +
                              ",CatID" +
                              ",vCategoryCode" +
                              ",vCategoryDesc" +
                              ",vCategoryShortDesc" +
                              ",vComments" +
                              ",vStatus" +
                              ",dStartDt" +
                              ",dEndDt" +
                              ",vEntryBy" +
                              ",tTimestamp" +

                          " FROM POS_ItemCategory"; // + " where vItemStatus='" + _vItemStatus + "'";


            //Connect to database and retrieve
            SettingsCategoryListForDisplay Obj = new SettingsCategoryListForDisplay();
            IList<SettingsCategoryListForDisplay> ALLcategory;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            ALLcategory = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);

            return ALLcategory;


        }

        public ObservableCollection<SettingsCategoryListForDisplay> SettingsCategoriesListDisplay() //List of Categories 
        {
            string _vStatus = "Publish";
            string SQL_str = "SELECT Id" +
                              ",CatID" +
                              ",vCategoryCode" +
                              ",vCategoryDesc" +
                              ",vCategoryShortDesc" +
                              ",vComments" +
                              ",vStatus" +
                              ",dStartDt" +
                              ",dEndDt" +
                              ",vEntryBy" +
                              ",tTimestamp" +

                          " FROM POS_ItemCategory"; // + " where vItemStatus='" + _vItemStatus + "'";


            //Connect to database and retrieve
            SettingsCategoryListForDisplay Obj = new SettingsCategoryListForDisplay();
            IList<SettingsCategoryListForDisplay> ALLcategoryList;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            ALLcategoryList = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);
            ObservableCollection<SettingsCategoryListForDisplay> ALLcategory = new ObservableCollection<SettingsCategoryListForDisplay>(ALLcategoryList);
            return ALLcategory;


        }
        public ObservableCollection<SettingsCategoryListForDisplay> SettingsCategoriesListDisplay(string inputParam) //List of Categories 
        {
            string _vStatus = "Publish";
            string SQL_str = "SELECT Id" +
                              ",CatID" +
                              ",vCategoryCode" +
                              ",vCategoryDesc" +
                              ",vCategoryShortDesc" +
                              ",vComments" +
                              ",vStatus" +
                              ",dStartDt" +
                              ",dEndDt" +
                              ",vEntryBy" +
                              ",tTimestamp" +

                          " FROM POS_ItemCategory"; // + " where vItemStatus='" + _vItemStatus + "'";


            //Connect to database and retrieve
            SettingsCategoryListForDisplay Obj = new SettingsCategoryListForDisplay();
            IList<SettingsCategoryListForDisplay> ALLcategoryList;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            ALLcategoryList = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);
            ObservableCollection<SettingsCategoryListForDisplay> ALLcategory = new ObservableCollection<SettingsCategoryListForDisplay>(ALLcategoryList);
            return ALLcategory;


        }
        public IList<SettingsListForDisplay> SettingsListDisplayAdvance() //List of Settings 
        {
            string _vItemStatus = "Publish";
            string SQL_str = "SELECT item.Id" +
                              ",itemID" +
                              ",vItemSku" +
                              ",vItemDesc1" +
                              ",vItemDesc2" +
                              ",vItemPrice" +
                              ",vItemRelatedID" +
                              ",vItemMinPrice" +
                              ",vItemVendor" +
                              ",vItemVendorContact" +
                              ",vItemVendorPhone" +
                              ",vItemNotes" +
                              ",vItemStatus" +
                              ",vItemAvailability" +
                              ",item.dStartDate" +
                              ",item.dEndDate" +
                              ",item.vEntryBy" +
                              ",item.tTimestamp" +
                              ",POS_ItemCategoryId" +
                              ",vCategoryCode" +
                              ",CatID" +
                          " FROM POS_ItemMaster item,POS_ItemCategory itemCat" +
                          " where item.pos_itemCategoryId=itemCat.ID and  vItemStatus='" + _vItemStatus + "'";


            //Connect to database and retrieve
            SettingsListForDisplay Obj = new SettingsListForDisplay();
            IList<SettingsListForDisplay> ALLitems;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            ALLitems = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);

            return ALLitems;


        }
        //public bool SettingsUpdate(SettingsListForDisplay InputRecordObj) //Update Settings 
        //{
        //    string _vItemStatus = "Publish";
        //    string SQL_str = "Update POS_ItemMaster SET " +
        //                      "[vItemSku]=@vItemSku" +
        //                      ",[vItemDesc1]=@vItemDesc1" +
        //                      ",[vItemDesc2]=@vItemDesc2" +
        //                      ",[vItemPrice]=@vItemPrice" +
        //                      ",[vItemRelatedID]=@vItemRelatedID" +
        //                      ",[vItemMinPrice]=@vItemMinPrice" +
        //                      ",[vItemVendor]=@vItemVendor" +
        //                      ",[vItemVendorContact]=@vItemVendorContact" +
        //                      ",[vItemVendorPhone]=@vItemVendorPhone" +
        //                      ",[vItemNotes]=@vItemNotes" +
        //                      ",[vItemStatus]=@vItemStatus" +
        //                      ",[vItemAvailability]=@vItemAvailability" +
        //                      ",[dStartDate]=@dStartDate" +
        //                      ",[dEndDate]=@dEndDate" +
        //                      ",[vEntryBy]=@vEntryBy" +
        //                      ",[tTimestamp]=@tTimestamp" +
        //                      " Where ItemID = " + InputRecordObj.itemID;
        //    //Connect to database and retrieve
        //    SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();


        //    POSdbAccess.connection.Execute(SQL_str, new
        //    {
        //        InputRecordObj.vItemSku
        //                      ,
        //        InputRecordObj.vItemDesc1
        //                      ,
        //        InputRecordObj.vItemDesc2
        //                      ,
        //        InputRecordObj.vItemPrice
        //                      ,
        //        InputRecordObj.vItemRelatedID
        //                      ,
        //        InputRecordObj.vItemMinPrice
        //                      ,
        //        InputRecordObj.vItemVendor
        //                      ,
        //        InputRecordObj.vItemVendorContact
        //                      ,
        //        InputRecordObj.vItemVendorPhone
        //                      ,
        //        InputRecordObj.vItemNotes
        //                      ,
        //        InputRecordObj.vItemStatus
        //                      ,
        //        InputRecordObj.vItemAvailability
        //                      ,
        //        InputRecordObj.dStartDate
        //                      ,
        //        InputRecordObj.dEndDate
        //                      ,
        //        InputRecordObj.vEntryBy
        //                      ,
        //        InputRecordObj.tTimestamp
        //    });
        //    bool Result;
        //    Result = true;
        //    // DataTable ResultTable;

        //    // Result = POSdbAccess.execScalar(SQL_str);
        //    //ResultTable = ResultSet.Tables[0];

        //    return Result;


        //}
        public IList<SettingsListForDisplay> SettingsListDisplayAdvance(string paramCategory) //List of Items 
        {
            string _vItemStatus = "Publish";
            string _querySign = "=";
            if (paramCategory == "All Items")
                _querySign = "<>";
            string SQL_str = "SELECT item.Id" +
                              ",itemID" +
                              ",vItemSku" +
                              ",vItemDesc1" +
                              ",vItemDesc2" +
                              ",vItemPrice" +
                              ",vItemRelatedID" +
                              ",vItemMinPrice" +
                              ",vItemVendor" +
                              ",vItemVendorContact" +
                              ",vItemVendorPhone" +
                              ",vItemNotes" +
                              ",vItemStatus" +
                              ",vItemAvailability" +
                              ",item.dStartDate" +
                              ",item.dEndDate" +
                              ",item.vEntryBy" +
                              ",item.tTimestamp" +
                              ",POS_ItemCategoryId" +
                              ",vCategoryCode" +
                              ",CatID" +
                          " FROM POS_ItemMaster item,POS_ItemCategory itemCat" +
                          " where item.pos_itemCategoryId=itemCat.ID and  vItemStatus='" + _vItemStatus + "'" +
                          " and itemCat.vCategoryShortDesc" + _querySign + "'" + paramCategory + "'";


            //Connect to database and retrieve
            SettingsListForDisplay Obj = new SettingsListForDisplay();
            IList<SettingsListForDisplay> ALLitems;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            ALLitems = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);

            return ALLitems;


        }
    }
}
