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

    public class ItemListForDisplay :NotifyProperty
    {
       public Int64 Id { get; set; }
       public Int64 itemID { get; set; }
        public string vItemSku{ get; set; }
        public string vItemDesc1 { get; set; }
        public string vItemDesc2 { get; set; }
        public string vItemPrice { get; set; }
        public string vItemRelatedID { get; set; }
        public string vItemMinPrice { get; set; }
        public string vItemVendor { get; set; }
        public string vItemVendorContact { get; set; }
        public string vItemVendorPhone { get; set; }
        public string vItemNotes { get; set; }
        public string vItemStatus { get; set; }
        public string vItemAvailability { get; set; }
        public string dStartDate { get; set; }
        public string dEndDate { get; set; }
        public string vEntryBy { get; set; }
        public string tTimestamp { get; set; }
        public string POS_ItemCategoryId { get; set; }
        public string vCategoryCode{ get; set; }
        public string vCategoryShortDesc { get; set; }
        public string CatID { get; set; }
        public string vItemmodifier { get; set; }

        public string ActionMode { get; set; }
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
   
    public class ItemCategoryListForDisplay : NotifyProperty
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
    class ItemsBuilder : SqlCEDataAccess
    {
        
        
        public DataTable POSitems(string EntityType)
        {
            if (EntityType == "Items")
                return ItemList();
            else
                return ItemList();

        }
        public Int32 RetrieveNextItemID() //Next Item id 
        {
            string _vItemStatus = "Publish";
            string SQL_str = "SELECT max([itemID]) ItemID " +
                            "FROM [POS_ItemMaster]" +
                            "where itemID< 9000 ";

            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;
            
            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSitemMasterItemID");
            ResultTable = ResultSet.Tables[0];
            DataRow dr;
            dr = ResultTable.Rows[0];
            Int32 NextItemNumber=1;
            if (ResultTable.Rows.Count > 0)
            {
                NextItemNumber = ResultTable.Rows[0].Field<Int32>("ItemID"); //.ToString();
                NextItemNumber++;
            }
            return NextItemNumber;


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

        public IList<ItemListForDisplay> ItemListDisplay() //List of Items 
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
                              ",vItemmodifier" +
                          " FROM POS_ItemMaster" + " where vItemStatus='" + _vItemStatus + "'";


            //Connect to database and retrieve
            ItemListForDisplay Obj = new ItemListForDisplay();
            IList<ItemListForDisplay> ALLitems;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
           ALLitems= POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);
            
            return ALLitems;


        }
        public IList<ItemCategoryListForDisplay> ItemCategoryListDisplay() //List of Items 
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
            ItemCategoryListForDisplay Obj = new ItemCategoryListForDisplay();
            IList<ItemCategoryListForDisplay> ALLcategory;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            ALLcategory = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);

            return ALLcategory;


        }

        public ObservableCollection<ItemCategoryListForDisplay> ItemCategoriesListDisplay() //List of Categories 
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
            ItemCategoryListForDisplay Obj = new ItemCategoryListForDisplay();
            IList<ItemCategoryListForDisplay> ALLcategoryList;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            ALLcategoryList =  POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);
            ObservableCollection<ItemCategoryListForDisplay> ALLcategory = new ObservableCollection<ItemCategoryListForDisplay>(ALLcategoryList);
            return ALLcategory;


        }
        public ObservableCollection<ItemCategoryListForDisplay> ItemCategoriesListDisplay(string inputParam) //List of Categories 
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
            ItemCategoryListForDisplay Obj = new ItemCategoryListForDisplay();
            IList<ItemCategoryListForDisplay> ALLcategoryList;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            ALLcategoryList = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);
            ObservableCollection<ItemCategoryListForDisplay> ALLcategory = new ObservableCollection<ItemCategoryListForDisplay>(ALLcategoryList);
            return ALLcategory;


        }
        public IList<ItemListForDisplay> ItemListDisplayAdvance() //List of Items 
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
                              ",vCategoryCode"+
                              ",vCategoryShortDesc"+
                              ",CatID"+
                              ",vItemmodifier" +
                          " FROM POS_ItemMaster item,POS_ItemCategory itemCat" + 
                          " where item.pos_itemCategoryId=itemCat.ID and  vItemStatus='" + _vItemStatus + "'";


            //Connect to database and retrieve
            ItemListForDisplay Obj = new ItemListForDisplay();
            IList<ItemListForDisplay> ALLitems;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            ALLitems = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);

            return ALLitems;


        }
        public bool ItemUpdate(ItemListForDisplay InputRecordObj) //Update Item 
        {
            string _vItemStatus = "Publish";
            string SQL_str = "Update POS_ItemMaster SET " +
                              "[vItemSku]=@vItemSku" +
                              ",[vItemDesc1]=@vItemDesc1" +
                              ",[vItemDesc2]=@vItemDesc2" +
                              ",[vItemPrice]=@vItemPrice" +
                              ",[vItemRelatedID]=@vItemRelatedID" +
                              ",[vItemMinPrice]=@vItemMinPrice" +
                              ",[vItemVendor]=@vItemVendor" +
                              ",[vItemVendorContact]=@vItemVendorContact" +
                              ",[vItemVendorPhone]=@vItemVendorPhone" +
                              ",[vItemNotes]=@vItemNotes" +
                              ",[vItemStatus]=@vItemStatus" +
                              ",[vItemAvailability]=@vItemAvailability" +
                              ",[dStartDate]=@dStartDate" +
                              ",[dEndDate]=@dEndDate" +
                              ",[vEntryBy]=@vEntryBy" +
                              ",[tTimestamp]=@tTimestamp" +
                              ",[vItemmodifier]=@vItemmodifier" +
                              " Where ItemID = " + InputRecordObj.itemID;
            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();


            POSdbAccess.connection.Execute(SQL_str, new
            {
                InputRecordObj.vItemSku
                              ,InputRecordObj.vItemDesc1
                              ,InputRecordObj.vItemDesc2
                              ,InputRecordObj.vItemPrice
                              ,InputRecordObj.vItemRelatedID
                              ,InputRecordObj.vItemMinPrice
                              ,InputRecordObj.vItemVendor
                              ,InputRecordObj.vItemVendorContact
                              ,InputRecordObj.vItemVendorPhone
                              ,InputRecordObj.vItemNotes
                              ,InputRecordObj.vItemStatus
                              ,InputRecordObj.vItemAvailability
                              ,InputRecordObj.dStartDate
                              ,InputRecordObj.dEndDate
                              ,InputRecordObj.vEntryBy
                              ,InputRecordObj.tTimestamp
                              ,InputRecordObj.vItemmodifier
            });
            bool Result;
            Result = true;
            // DataTable ResultTable;

            // Result = POSdbAccess.execScalar(SQL_str);
            //ResultTable = ResultSet.Tables[0];

            return Result;


        }
        public bool ItemAdd(ItemListForDisplay InputRecordObj) //Add Item 
        {

            InputRecordObj.itemID = RetrieveNextItemID();
            
            string SQL_str = "Insert into POS_ItemMaster ([itemID],[vItemSku],[vItemDesc1]," +
                                "[vItemDesc2],"+
                                "[vItemPrice]," +
                                "[vItemRelatedID]," +
                                "[vItemMinPrice]," +
                                "[vItemVendor]," +
                                "[vItemVendorContact]," +
                                "[vItemVendorPhone]," +
                                "[vItemNotes]," +
                                "[vItemStatus]," +
                                "[vItemAvailability]," +
                                "[dStartDate]," +
                                "[dEndDate]," +
                                "[vEntryBy]," +
                                "[tTimestamp]," +
                                "[POS_ItemCategoryid]," +
                                "[vItemmodifier]" +
                                ") values (@itemID" +
                              ",@vItemSku" +
                              ",@vItemDesc1" +
                              ",@vItemDesc2" +
                              ",@vItemPrice" +
                              ",@vItemRelatedID" +
                              ",@vItemMinPrice" +
                              ",@vItemVendor" +
                              ",@vItemVendorContact" +
                              ",@vItemVendorPhone" +
                              ",@vItemNotes" +
                              ",@vItemStatus" +
                              ",@vItemAvailability" +
                              ",@dStartDate" +
                              ",@dEndDate" +
                              ",@vEntryBy" +
                              ",@tTimestamp" +
                              ",@POS_ItemCategoryid" +
                              ",@vItemmodifier" +
                              ")";
            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();


            POSdbAccess.connection.Execute(SQL_str, new
            {
                InputRecordObj.itemID,
                InputRecordObj.vItemSku,
                InputRecordObj.vItemDesc1,
                InputRecordObj.vItemDesc2,
                InputRecordObj.vItemPrice,
                InputRecordObj.vItemRelatedID,
                InputRecordObj.vItemMinPrice,
                InputRecordObj.vItemVendor,
                InputRecordObj.vItemVendorContact,
                InputRecordObj.vItemVendorPhone,
                InputRecordObj.vItemNotes,
                InputRecordObj.vItemStatus,
                InputRecordObj.vItemAvailability,
                InputRecordObj.dStartDate,
                InputRecordObj.dEndDate,
                InputRecordObj.vEntryBy,
                InputRecordObj.tTimestamp,
                InputRecordObj.POS_ItemCategoryId,
                InputRecordObj.vItemmodifier
            });
            bool Result;
            Result = true;
          
            
            return Result;


        }
        //this is called from Item Module.
        public IList<ItemListForDisplay> ItemListDisplayAdvance(string paramCategory) //List of Items 
        {
            string _vItemStatus = "Publish";
            string _querySign="=";
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
                              ",vCategoryShortDesc"+
                              ",CatID" +
                              ",vItemmodifier" +
                          " FROM POS_ItemMaster item,POS_ItemCategory itemCat" +
                          " where item.pos_itemCategoryId=itemCat.ID and  vItemStatus='" + _vItemStatus + "'"+
                          " and itemCat.vCategoryShortDesc"+_querySign+"'" + paramCategory +"'";


            //Connect to database and retrieve
            ItemListForDisplay Obj = new ItemListForDisplay();
            IList<ItemListForDisplay> ALLitems;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            ALLitems = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);

            return ALLitems;


        }
    }
}
