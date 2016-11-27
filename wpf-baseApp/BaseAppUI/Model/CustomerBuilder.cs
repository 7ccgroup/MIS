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
  
       

        public class CustomerListForDisplay : NotifyProperty
        {
            public Int64  Id { get; set; }
            public Int64 iCustid { get; set; }
            public string vCustName { get; set; }
            public string vCustContactNme { get; set; }
            public string vCustPrimaryPh { get; set; }
            public string vCustPhone2 { get; set; }
            public string vCustPhone3 { get; set; }
            public string vCustPhone4 { get; set; }
            public string vCustFax1 { get; set; }
            public string vCustFax2 { get; set; }
            public string vCustEmail { get; set; }
            public string vCustAddress1 { get; set; }
            public string vCustAddress2 { get; set; }
            public string vCustCity { get; set; }
            public string vCustState { get; set; }
            public string vCustCountry { get; set; }
            public string vCustZipCode { get; set; }
            public string vCustBillAddress1 { get; set; }
            public string vCustBillAddress2 { get; set; }
            public string vCustBillCity { get; set; }
            public string vCustBillCountry { get; set; }
            public string vCustBillZipCode { get; set; }
            public string vCustShipAddress1 { get; set; }
            public string vCustShipAddress2 { get; set; }
            public string vCustShipCity { get; set; }
            public string vCustShipState { get; set; }
            public string vCustShipCountry { get; set; }
            public string vCustShipZipCode { get; set; }
            public string vCustAccountNum { get; set; }
            public string vCustNote1 { get; set; }
            public string vCustNote2 { get; set; }
            public string vCustNote3 { get; set; }
            public string vCustCategory { get; set; }
            public string vCustComments { get; set; }
            public string vCustStatus { get; set; }
            public string vEntryBy { get; set; }
            public string tTimestamp { get; set; }
            public string CatID { get; set; }

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

    /*    public class ItemCategoryListForDisplay : NotifyProperty
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
        } */
        class CustomersBuilder : SqlCEDataAccess
        {


            
            public bool CustomerAdd(CustomerListForDisplay InputRecordObj) //Add Customer 
            {
                string _vItemStatus = "Publish";
                string SQL_str = "Insert Into POS_Customer (id" +
                                  ",iCustid" +
                                  ",vCustName" +
                                  ",vCustContactNme" +
                                  ",vCustPrimaryPh" +
                                  ",vCustPhone2" +
                                  ",vCustPhone3" +
                                  ",vCustPhone4" +
                                  ",vCustFax1" +
                                  ",vCustFax2" +
                                  ",vCustEmail" +
                                  ",vCustAddress1" +
                                  ",vCustAddress2" +
                                  ",vCustCity" +
                                  ",vCustState" +
                                  ",vCustCountry" +
                                  ",vCustZipCode" +
                                  ",vCustBillAddress1" +
                                  ",vCustBillAddress2" +
                                  ",vCustBillCity" +
                                  ",vCustBillCountry" +
                                  ",vCustBillZipCode" +
                                  ",vCustShipAddress1" +
                                  ",vCustShipAddress2" +
                                  ",vCustShipCity" +
                                  ",vCustShipState" +
                                  ",vCustShipCountry" +
                                  ",vCustShipZipCode" +
                                  ",vCustAccountNum" +
                                  ",vCustNote1" +
                                  ",vCustNote2" +
                                  ",vCustNote3" +
                                  ",vCustCategory" +
                                  ",vCustComments" +
                                  ",vCustStatus" +
                                  ",vEntryBy" +
                                  ",tTimestamp" +
                                  ",POS_Setup_Id)" +
                                    "Values ( " + "@id" +
                                  ",@iCustid" +
                                  ",@vCustName" +
                                  ",@vCustContactNme" +
                                  ",@vCustPrimaryPh" +
                                  ",@vCustPhone2" +
                                  ",@vCustPhone3" +
                                  ",@vCustPhone4" +
                                  ",@vCustFax1" +
                                  ",@vCustFax2" +
                                  ",@vCustEmail" +
                                  ",@vCustAddress1" +
                                  ",@vCustAddress2" +
                                  ",@vCustCity" +
                                  ",@vCustState" +
                                  ",@vCustCountry" +
                                  ",@vCustZipCode" +
                                  ",@vCustBillAddress1" +
                                  ",@vCustBillAddress2" +
                                  ",@vCustBillCity" +
                                  ",@vCustBillCountry" +
                                  ",@vCustBillZipCode" +
                                  ",@vCustShipAddress1" +
                                  ",@vCustShipAddress2" +
                                  ",@vCustShipCity" +
                                  ",@vCustShipState" +
                                  ",@vCustShipCountry" +
                                  ",@vCustShipZipCode" +
                                  ",@vCustAccountNum" +
                                  ",@vCustNote1" +
                                  ",@vCustNote2" +
                                  ",@vCustNote3" +
                                  ",@vCustCategory" +
                                  ",@vCustComments" +
                                  ",@vCustStatus" +
                                  ",@vEntryBy" +
                                  ",@tTimestamp" +
                                  ",@POS_Setup_Id)" ;


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

                bool Result;
                DataTable ResultTable;

            Result = POSdbAccess.execScalar(SQL_str);
                //ResultTable = ResultSet.Tables[0];

                return Result;


            }
        public bool CustomerUpdate(CustomerListForDisplay InputRecordObj) //Update Customer 
        {
            string _vItemStatus = "Publish";
            string SQL_str = "Update POS_Customer set " +
                                  "[vCustName]=@vCustName" +
                                  ",[vCustContactNme]=@vCustContactNme" +
                                  ",[vCustPrimaryPh]=@vCustPrimaryPh" +
                                  ",[vCustPhone2]=@vCustPhone2" +
                                  ",[vCustPhone3]=@vCustPhone3" +
                                  ",[vCustPhone4]=@vCustPhone4" +
                                  ",[vCustFax1]=@vCustFax1" +
                                  ",[vCustFax2]=@vCustFax2" +
                                  ",[vCustEmail]=@vCustEmail" +
                                  ",[vCustAddress1]=@vCustAddress1" +
                                  ",[vCustAddress2]=@vCustAddress2" +
                                  ",[vCustCity]=@vCustCity" +
                                  ",[vCustState]=@vCustState" +
                                  ",[vCustCountry]=@vCustCountry" +
                                  ",[vCustZipCode]=@vCustZipCode" +
                                  ",[vCustBillAddress1]=@vCustBillAddress1" +
                                  ",[vCustBillAddress2]=@vCustBillAddress2" +
                                  ",[vCustBillCity]=@vCustBillCity" +
                                  ",[vCustBillCountry]=@vCustBillCountry" +
                                  ",[vCustBillZipCode]=@vCustBillZipCode" +
                                  ",[vCustShipAddress1]=@vCustShipAddress1" +
                                  ",[vCustShipAddress2]=@vCustShipAddress2" +
                                  ",[vCustShipCity]=@vCustShipCity" +
                                  ",[vCustShipState]=@vCustShipState" +
                                  ",[vCustShipCountry]=@vCustShipCountry" +
                                  ",[vCustShipZipCode]=@vCustShipZipCode" +
                                  ",[vCustAccountNum]=@vCustAccountNum" +
                                  ",[vCustNote1]=@vCustNote1" +
                                  ",[vCustNote2]=@vCustNote2" +
                                  ",[vCustNote3]=@vCustNote3" +
                                  ",[vCustCategory]=@vCustCategory" +
                                  ",[vCustComments]=@vCustComments" +
                                  ",[vCustStatus]=@vCustStatus" +
                                  ",[vEntryBy]=@vEntryBy" +
                                  ",[tTimestamp]=@tTimestamp" +
                                   " Where id = " + InputRecordObj.Id;
            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            

            POSdbAccess.connection.Execute(SQL_str, new {InputRecordObj.vCustName
               ,InputRecordObj.iCustid
                                  ,
                 
                InputRecordObj.vCustContactNme
                                  ,
                InputRecordObj.vCustPrimaryPh
                                  ,
                InputRecordObj.vCustPhone2
                                  ,
                InputRecordObj.vCustPhone3
                                  ,
                InputRecordObj.vCustPhone4
                                  ,
                InputRecordObj.vCustFax1
                                  ,
                InputRecordObj.vCustFax2
                                  ,
                InputRecordObj.vCustEmail
                                  ,
                InputRecordObj.vCustAddress1
                                  ,
                InputRecordObj.vCustAddress2
                                  ,
                InputRecordObj.vCustCity
                                  ,
                InputRecordObj.vCustState
                                  ,
                InputRecordObj.vCustCountry
                                  ,
                InputRecordObj.vCustZipCode
                                  ,
                InputRecordObj.vCustBillAddress1
                                  ,
                InputRecordObj.vCustBillAddress2
                                  ,
                InputRecordObj.vCustBillCity
                                  ,
                InputRecordObj.vCustBillCountry
                                  ,
                InputRecordObj.vCustBillZipCode
                                  ,
                InputRecordObj.vCustShipAddress1
                                  ,
                InputRecordObj.vCustShipAddress2
                                  ,
                InputRecordObj.vCustShipCity
                                  ,
                InputRecordObj.vCustShipState
                                  ,
                InputRecordObj.vCustShipCountry
                                  ,
                InputRecordObj.vCustShipZipCode
                                  ,
                InputRecordObj.vCustAccountNum
                                  ,
                InputRecordObj.vCustNote1
                                  ,
                InputRecordObj.vCustNote2
                                  ,
                InputRecordObj.vCustNote3
                                  ,
                InputRecordObj.vCustCategory
                                  ,
                InputRecordObj.vCustComments
                                  ,
                InputRecordObj.vCustStatus
                                  ,
                InputRecordObj.vEntryBy
                                  ,
                InputRecordObj.tTimestamp
                                  
            });
            bool Result;
            Result = true;
           // DataTable ResultTable;

           // Result = POSdbAccess.execScalar(SQL_str);
            //ResultTable = ResultSet.Tables[0];

            return Result;


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
                              " FROM POS_ItemMaster" + " where vItemStatus='" + _vItemStatus + "'";


                //Connect to database and retrieve
                ItemListForDisplay Obj = new ItemListForDisplay();
                IList<ItemListForDisplay> ALLitems;
                SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
                ALLitems = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);

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
                ALLcategoryList = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);
                ObservableCollection<ItemCategoryListForDisplay> ALLcategory = new ObservableCollection<ItemCategoryListForDisplay>(ALLcategoryList);
                return ALLcategory;


            }
    

            public IList<CustomerListForDisplay> CustomerListDisplayAdvance(string paramCategory) //List of customers 
            {
             
               
                string SQL_str = "Select id" +
                                  ",iCustid" +
                                  ",vCustName" +
                                  ",vCustContactNme" +
                                  ",vCustPrimaryPh" +
                                  ",vCustPhone2" +
                                  ",vCustPhone3" +
                                  ",vCustPhone4" +
                                  ",vCustFax1" +
                                  ",vCustFax2" +
                                  ",vCustEmail" +
                                  ",vCustAddress1" +
                                  ",vCustAddress2" +
                                  ",vCustCity" +
                                  ",vCustState" +
                                  ",vCustCountry" +
                                  ",vCustZipCode" +
                                  ",vCustBillAddress1" +
                                  ",vCustBillAddress2" +
                                  ",vCustBillCity" +
                                  ",vCustBillCountry" +
                                  ",vCustBillZipCode" +
                                  ",vCustShipAddress1" +
                                  ",vCustShipAddress2" +
                                  ",vCustShipCity" +
                                  ",vCustShipState" +
                                  ",vCustShipCountry" +
                                  ",vCustShipZipCode" +
                                  ",vCustAccountNum" +
                                  ",vCustNote1" +
                                  ",vCustNote2" +
                                  ",vCustNote3" +
                                  ",vCustCategory" +
                                  ",vCustComments" +
                                  ",vCustStatus" +
                                  ",vEntryBy" +
                                  ",tTimestamp" +
                                  ",POS_Setup_Id" +
                                    " FROM POS_Customer" ;


                //Connect to database and retrieve
                CustomerListForDisplay Obj = new CustomerListForDisplay();
                IList<CustomerListForDisplay> ALLcustomers;
                SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
                ALLcustomers = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);

                return ALLcustomers;


            }
        }
    }

  

