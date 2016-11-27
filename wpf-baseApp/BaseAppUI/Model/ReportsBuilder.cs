using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BaseAppData;
using System.Windows;
using System.Collections.ObjectModel;

namespace BaseAppUI.Model
{
   public class SalesForDataVisual : NotifyProperty
    {
        public string OrderDate {get;set;}
        public float Sales { get; set; }
    }   
    class ReportsBuilder
    {
        String TransactionListDate;
        string _cReportType;
        public string CreportType
        {
            get { return _cReportType; }
            set { _cReportType = value; }
        }
        public DataTable POSreports(string reportType)   //use this if datatable is needed.
        {
           if(CreportType !=null)
                MessageBox.Show(CreportType);
            
            if (reportType == "Items")
                return ItemList();
            if (reportType == "Customer")
                return CustomerList();
            if (reportType == "Pricing")
                return PriceList();
            if (reportType == "Inventory")
                return ItemOnStockList();
            if (reportType == "TopSellingItems")
                return Top5SellingItems();
            if (reportType == "TopSellingDays")
                return Top5SellingDays();
            if (reportType == "12MonthsSales")
                return Sales12Months();
            else
                return ItemList();

        }
        
        public String POSmainreports(string reportType)
        {
            if (reportType == "SalesTipsToday")
                return SalesTipToday("", OrderStatuses.Completed);
            if (reportType == "SalesGratuityToday")
                return SalesGratuityToday("", OrderStatuses.Completed);
            if (reportType == "SalesToday")
                return SalesToday();
            if (reportType == "CompletedSalesToday")
                return SalesToday("",OrderStatuses.Completed);
            if (reportType == "PendingSalesToday")
                return SalesToday("", OrderStatuses.Pending);
            if (reportType == "RefundSalesToday")
                return SalesToday("", OrderStatuses.Refunded);
            if (reportType == "CashSalesToday")
                return SalesToday(OrderPaymentTypes.Cash);
            if (reportType == "CreditSalesToday")
                return SalesToday(OrderPaymentTypes.Card);
            if (reportType == "OrdersToday")              
                return OrdersToday("Completed");
            if (reportType == "PendingOrdersToday")
                return OrdersToday("Pending");
            if (reportType == "ItemsCount")
                return ItemsCount();
            if (reportType == "CustomersCount")
                return CustomersCount();
            if (reportType == "MTDsales")
                return MTDsales();
            if (reportType == "YTDSales")
                return YTDSales();
            else
                return "";
        }
        public ObservableCollection<SalesForDataVisual> SalesForDataVisual() //List of sales  
        {
            string _vStatus = "Publish";   //
            //+ sum(dOrderTax) Sales ///not to be added in dorderamount


            string SQL_str = "select Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + " +
                        "Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        " DatePart(day, dCreatedDate)) OrderDate,sum(dOrderAmount) Sales  " +
                        " from pos_orderheader where Convert(nvarchar(10), DatePart(month, dCreatedDate))= '" + System.DateTime.Today.Month + "'" +
                        " and Convert(nvarchar(10), DatePart(year, dCreatedDate))= '" + System.DateTime.Today.Year + "'" +

                        " group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) +'-' + " +
                        " Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        " DatePart(day, dCreatedDate)) order by sales desc";



            //Connect to database and retrieve
            SalesForDataVisual Obj = new SalesForDataVisual();
            IList<SalesForDataVisual> SalesByMonthList;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            SalesByMonthList = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);
            ObservableCollection<SalesForDataVisual> SalesByDay = new ObservableCollection<SalesForDataVisual>(SalesByMonthList);
            return SalesByDay;


        }
        public ObservableCollection<SalesForDataVisual> MonthlySalesForDataVisual() //List of sales  
        {
            string _vStatus = "Publish";
            //+ sum(dOrderTax) Sales ///not to be added in dorderamount


            string SQL_str = "select Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + " +
                        "Convert(nvarchar(10), DatePart(month, dCreatedDate))  " +
                        "  OrderDate,sum(dOrderAmount) Sales  " +
                        " from pos_orderheader where "+
                        " Convert(nvarchar(10), DatePart(year, dCreatedDate))= '" + System.DateTime.Today.Year + "'" +

                        " group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) +'-' + " +
                        " Convert(nvarchar(10), DatePart(month, dCreatedDate))  " +
                        "  order by sales desc";



            //Connect to database and retrieve
            SalesForDataVisual Obj = new SalesForDataVisual();
            IList<SalesForDataVisual> SalesByMonthList;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            SalesByMonthList = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);
            ObservableCollection<SalesForDataVisual> SalesByMonth = new ObservableCollection<SalesForDataVisual>(SalesByMonthList);
            return SalesByMonth;


        }
        public ObservableCollection<SalesForDataVisual> WeeklyPaymentTrendVisual() //Last 7 days payment Trend.
        {
            string _vStatus = "Publish";
            //+ sum(dOrderTax) Sales ///not to be added in dorderamount


            string SQL_str = "select Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + " +
                        "Convert(nvarchar(10), DatePart(month, dCreatedDate))  " +
                        "  OrderDate,sum(dOrderAmount) Sales  " +
                        " from pos_orderheader where " +
                        " Convert(nvarchar(10), DatePart(year, dCreatedDate))= '" + System.DateTime.Today.Year + "'" +

                        " group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) +'-' + " +
                        " Convert(nvarchar(10), DatePart(month, dCreatedDate))  " +
                        "  order by sales desc";



            //Connect to database and retrieve
            SalesForDataVisual Obj = new SalesForDataVisual();
            IList<SalesForDataVisual> SalesByMonthList;
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();
            SalesByMonthList = POSdbAccess.connection.ConvertSqlQueryToIList(Obj, SQL_str);
            ObservableCollection<SalesForDataVisual> SalesByMonth = new ObservableCollection<SalesForDataVisual>(SalesByMonthList);
            return SalesByMonth;


        }

        public DataTable Sales12Months() //Sales12Months
        {
            //+ sum(dOrderTax) Sales ///not to be added in dorderamount

            string SQL_str = "select top(12) Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + " +
                        "Convert(nvarchar(10), DatePart(month, dCreatedDate)) " +
                        " OrderDate,sum(dOrderAmount) Sales  " +
                        " from pos_orderheader where Convert(nvarchar(10), DatePart(month, dCreatedDate))= '" + System.DateTime.Today.Month + "'" +
                        " and Convert(nvarchar(10), DatePart(year, dCreatedDate))= '" + System.DateTime.Today.Year + "'" +

                        " group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) +'-' + " +
                        " Convert(nvarchar(10), DatePart(month, dCreatedDate))  " +
                        " order by sales desc";

            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POStopDays");
            ResultTable = ResultSet.Tables[0];

            return ResultTable;


        }
        public DataTable Top5SellingDays() //Top5SellingItems
        {
            //+ sum(dOrderTax) Sales ///not to be added in dorderamount

            string SQL_str = "select top(5) Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + " +
                        "Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        " DatePart(day, dCreatedDate)) OrderDate,sum(dOrderAmount) Sales  " +
                        " from pos_orderheader where Convert(nvarchar(10), DatePart(month, dCreatedDate))= '" + System.DateTime.Today.Month + "'" +
                        " and Convert(nvarchar(10), DatePart(year, dCreatedDate))= '" + System.DateTime.Today.Year + "'" +

                        " group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) +'-' + " +
                        " Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        " DatePart(day, dCreatedDate)) order by sales desc";

            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POStopDays");
            ResultTable = ResultSet.Tables[0];

            return ResultTable;


        }

        public DataTable Top5SellingItems() //Top5SellingItems
        {

            string SQL_str = "SELECT top(5) vItemDesc1 Item," +
                             " sum([fOrderQty]) Quantity " +
                                " FROM[POS_OrderDetails] Ord,[POS_ItemMaster] " +
                                "Item where Ord.POS_ItemMasterId = Item.id and " +
                                "Convert(nvarchar(10), DatePart(year, dCreatedDate)) = '" + System.DateTime.Today.Year + "'" +
                                " group by vItemDesc1 " +
                                " order by Quantity Desc";

            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POStopItems");
            ResultTable = ResultSet.Tables[0];

            return ResultTable;


        }
        public String SalesPendingToday() //SalesPendingToday
        {

            string SQL_str = "select Convert(nvarchar(10), " +
                        "DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        "DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))" +
                        " OrderDate,sum(dOrderAmount) Sales,sum(dOrderTax) Tax" +
                        " from pos_orderheader" +
                        " where Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate)) ='" + System.DateTime.Today.ToString("yyyy-M-dd") + "'" +
                        "and vOrderStatus='Pending' group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSsalesPendingToday");
            ResultTable = ResultSet.Tables[0];
            String RtnString;
            Decimal SalesValue, TaxValue;

            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                SalesValue = (Decimal)ResultTable.Rows[0]["Sales"];
                TaxValue = (Decimal)ResultTable.Rows[0]["Tax"];
                //  SalesValue = SalesValue + TaxValue; //tax included in dOrderAmount. 5/8/16 saa
                RtnString = "$ " + SalesValue.ToString("##,##.00");
            }
            else
            {
                RtnString = "$ 0.00";
            }
            return RtnString;


        }
        public String RefundSalesToday() //RefundSalesToday
        {

            string SQL_str = "select Convert(nvarchar(10), " +
                        "DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        "DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))" +
                        " OrderDate,sum(dOrderAmount) Sales,sum(dOrderTax) Tax" +
                        " from pos_orderheader" +
                        " where Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate)) ='" + System.DateTime.Today.ToString("yyyy-M-dd") + "'" +
                        "and vOrderStatus='Refunded' group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSrefundsToday");
            ResultTable = ResultSet.Tables[0];
            String RtnString;
            Decimal SalesValue, TaxValue;

            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                SalesValue = (Decimal)ResultTable.Rows[0]["Sales"];
                TaxValue = (Decimal)ResultTable.Rows[0]["Tax"];
                //  SalesValue = SalesValue + TaxValue; //tax included in dOrderAmount. 5/8/16 saa
                RtnString = "$ " + SalesValue.ToString("##,##.00");
            }
            else
            {
                RtnString = "$ 0.00";
            }
            return RtnString;


        }
        public String TransactionList() //Transaction List.
        {

            string SQL_str = "select Convert(nvarchar(10), " +
                        "DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        "DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))" +
                        " OrderDate,sum(dOrderAmount) Sales,sum(dOrderTax) Tax" +
                        " from pos_orderheader" +
                        " where Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate)) ='" + System.DateTime.Today.ToString("yyyy-M-dd") + "'" +
                        "and vOrderStatus='Pending' group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSsalesPendingToday");
            ResultTable = ResultSet.Tables[0];
            String RtnString;
            Decimal SalesValue, TaxValue;

            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                SalesValue = (Decimal)ResultTable.Rows[0]["Sales"];
                TaxValue = (Decimal)ResultTable.Rows[0]["Tax"];
                //    SalesValue = SalesValue + TaxValue;//tax included in dOrderAmount. 5/8/16 saa
                RtnString = "$ " + SalesValue.ToString("##,##.00");
            }
            else
            {
                RtnString = "$ 0.00";
            }
            return RtnString;


        }
        public String SalesToday() //SalesToday
        {

            string SQL_str = "select Convert(nvarchar(10), " +
                        "DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        "DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))" +
                        " OrderDate,sum(dOrderAmount) Sales,sum(dOrderTax) Tax" +
                        " from pos_orderheader" +
                        " where  Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate)) ='" + System.DateTime.Today.ToString("yyyy-M-d") + "'" +
                        "group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSsalesToday");
            ResultTable = ResultSet.Tables[0];
            String RtnString;
            Decimal SalesValue, TaxValue;

            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                SalesValue = (Decimal)ResultTable.Rows[0]["Sales"];
                TaxValue = (Decimal)ResultTable.Rows[0]["Tax"];
             //   SalesValue = SalesValue + TaxValue; //tax included in dOrderAmount. 5/8/16 saa
                RtnString = "$ " + SalesValue.ToString("##,##.00");
            }
            else
            {
                RtnString = "$ 0.00";
            }
            return RtnString;


        }
        public String SalesToday(string paymentType,string orderstatus) //SalesToday
        {

            string SQL_str = "select Convert(nvarchar(10), " +
                        "DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        "DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))" +
                        " OrderDate,sum(dOrderAmount) Sales,sum(dOrderTax) Tax" +
                        " from pos_orderheader" +
                        " where vOrderStatus='" + orderstatus + "' and Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate)) ='" + System.DateTime.Today.ToString("yyyy-M-d") + "'" +
                        "group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSsalesToday");
            ResultTable = ResultSet.Tables[0];
            String RtnString;
            Decimal SalesValue, TaxValue;

            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                SalesValue = (Decimal)ResultTable.Rows[0]["Sales"];
                TaxValue = (Decimal)ResultTable.Rows[0]["Tax"];
                //  SalesValue = SalesValue; // + TaxValue; Tax is included in dOrderAmount......5/8/16
                RtnString = "$ " + SalesValue.ToString("##,##.00");
            }
            else
            {
                RtnString = "$ 0.00";
            }
            return RtnString;


        }
        public String SalesGratuityToday(string paymentType, string orderstatus) //SalesToday
        {

            string SQL_str = "select Convert(nvarchar(10), " +
                        "DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        "DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))" +
                        " OrderDate,COALESCE(sum(dOrderServiceFee),0) SalesGratuity,sum(dOrderTax) Tax" +
                        " from pos_orderheader" +
                        " where vOrderStatus='" + orderstatus + "' and Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate)) ='" + System.DateTime.Today.ToString("yyyy-M-d") + "'" +
                        "group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSsalesTipsToday");
            ResultTable = ResultSet.Tables[0];
            String RtnString;
            Decimal SalesValue, TaxValue;

            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                if (ResultTable.Rows[0]["SalesGratuity"] != null)
                    SalesValue = (Decimal)ResultTable.Rows[0]["SalesGratuity"];
                else
                    SalesValue = 0;
                TaxValue = (Decimal)ResultTable.Rows[0]["Tax"];
                //  SalesValue = SalesValue; // + TaxValue; Tax is included in dOrderAmount......5/8/16
                RtnString = "$ " + SalesValue.ToString("##,##.00");
            }
            else
            {
                RtnString = "$ 0.00";
            }
            return RtnString;


        }

        public String SalesTipToday(string paymentType, string orderstatus) //SalesToday
        {

            string SQL_str = "select Convert(nvarchar(10), " +
                        "DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        "DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))" +
                        " OrderDate,COALESCE(sum(dOrderTip),0) SalesTips,sum(dOrderTax) Tax" +
                        " from pos_orderheader" +
                        " where vOrderStatus='" + orderstatus + "' and Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate)) ='" + System.DateTime.Today.ToString("yyyy-M-d") + "'" +
                        "group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSsalesTipsToday");
            ResultTable = ResultSet.Tables[0];
            String RtnString;
            Decimal SalesValue, TaxValue;

            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                SalesValue = (Decimal)ResultTable.Rows[0]["SalesTips"];
                TaxValue = (Decimal)ResultTable.Rows[0]["Tax"];
                //  SalesValue = SalesValue; // + TaxValue; Tax is included in dOrderAmount......5/8/16
                RtnString = "$ " + SalesValue.ToString("##,##.00");
            }
            else
            {
                RtnString = "$ 0.00";
            }
            return RtnString;


        }
        public String SalesToday(string paymentType) //SalesToday
        {

            string SQL_str = "select Convert(nvarchar(10), " +
                        "DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        "DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))" +
                        " OrderDate,sum(dOrderAmount) Sales,sum(dOrderTax) Tax" +
                        " from pos_orderheader" +
                        " where vOrderPaymentType='" + paymentType + "' and Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate)) ='" + System.DateTime.Today.ToString("yyyy-M-d") + "'" +
                        "group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSsalesToday");
            ResultTable = ResultSet.Tables[0];
            String RtnString;
            Decimal SalesValue, TaxValue;

            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                SalesValue = (Decimal)ResultTable.Rows[0]["Sales"];
                TaxValue = (Decimal)ResultTable.Rows[0]["Tax"];
              //  SalesValue = SalesValue; // + TaxValue; Tax is included in dOrderAmount......5/8/16
                RtnString = "$ " + SalesValue.ToString("##,##.00");
            }
            else
            {
                RtnString = "$ 0.00";
            }
            return RtnString;


        }
        public String MTDsales() //MTDSales
        {

            string SQL_str = "select " +
                        "Convert(nvarchar(10), " +
                        "DatePart(month, dCreatedDate))" +
                        " OrderDate,sum(dOrderAmount) Sales,sum(dOrderTax) Tax" +
                        " from pos_orderheader" +
                        " where Convert(nvarchar(10), DatePart(month, dCreatedDate)) ='" + System.DateTime.Today.Month + "'" +
                        " and Convert(nvarchar(10), DatePart(year, dCreatedDate)) ='" + System.DateTime.Today.Year + "'" +
                        "group by Convert(nvarchar(10), DatePart(month, dCreatedDate)) ";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSmtdSales");
            ResultTable = ResultSet.Tables[0];
            String RtnString;
            Decimal SalesValue, TaxValue;

            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                SalesValue = (Decimal)ResultTable.Rows[0]["Sales"];
                TaxValue = (Decimal)ResultTable.Rows[0]["Tax"];
                //     SalesValue = SalesValue + TaxValue; //tax included in dOrderAmount. 5/8/16 saa
                RtnString = "$ " + SalesValue.ToString("##,##.00");
            }
            else
            {
                RtnString = "$ 0.00";
            }
            return RtnString;


        }
        public String YTDSales() //MonthlyAvg
        {

            string SQL_str = "select " +
                        "Convert(nvarchar(10), " +
                        "DatePart(year, dCreatedDate))" +
                        " OrderDate,sum(dOrderAmount) Sales,sum(dOrderTax) Tax" +
                        " from pos_orderheader" +
                        " where Convert(nvarchar(10), DatePart(year, dCreatedDate)) ='" + System.DateTime.Today.Year + "'" +
                        "group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) ";
            Int16 MonthCount = (Int16)System.DateTime.Today.Month;

            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSytdSales");
            ResultTable = ResultSet.Tables[0];
            String RtnString;
            Decimal SalesValue, TaxValue;

            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                SalesValue = (Decimal)ResultTable.Rows[0]["Sales"];
                TaxValue = (Decimal)ResultTable.Rows[0]["Tax"];
                //    SalesValue = SalesValue + TaxValue; //tax included in dOrderAmount. 5/8/16 saa

                RtnString = "$ " + SalesValue.ToString("##,##.00");
            }
            else
            {
                RtnString = "$ 0.00";
            }
            return RtnString;


        }
        public String OrdersToday(string orderStatus) //OrdersToday
        {

            string SQL_str = "select Convert(nvarchar(10), " +
                        "DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), " +
                        "DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))" +
                        " OrderDate,Count(iOrderID) OrderCount" +
                        " from pos_orderheader" +
                        " where vOrderStatus ='"+orderStatus +"' and Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate)) ='" + System.DateTime.Today.ToString("yyyy-M-d") + "'" +
                        "group by Convert(nvarchar(10), DatePart(year, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(month, dCreatedDate)) + '-' + Convert(nvarchar(10), DatePart(day, dCreatedDate))";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSordersToday");
            ResultTable = ResultSet.Tables[0];
            String RtnString;


            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                //  OrderCount = (Int32)ResultTable.Rows[0]["OrderCount"];

                //  RtnString =  OrderCount.ToString();
                RtnString = ResultTable.Rows[0]["OrderCount"].ToString();
            }
            else
            {
                RtnString = "0";
            }
            return RtnString;


        }
       
        public String ItemsCount() //ItemsCount
        {

            string SQL_str = "select count(ItemID) ItemCount from POS_ItemMaster ";

            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSItemsCount");
            ResultTable = ResultSet.Tables[0];
            String RtnString;


            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                //  OrderCount = (Int32)ResultTable.Rows[0]["OrderCount"];

                //  RtnString =  OrderCount.ToString();
                RtnString = ResultTable.Rows[0]["ItemCount"].ToString();
            }
            else
            {
                RtnString = "0";
            }
            return RtnString;


        }
        public String CustomersCount() //ItemsCount
        {

            string SQL_str = "select count(iCustID) CustomerCount from POS_Customer";

            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSCustomerCount");
            ResultTable = ResultSet.Tables[0];
            String RtnString;


            //    SalesValue = (from DataRow dr in ResultTable.Rows where dr["Sales"] == EnvironmentVariableTarget select (int)dr["id"].FirstOrDefault);
            if (ResultTable.Rows.Count > 0 && ResultTable.Rows != null)
            {
                //  OrderCount = (Int32)ResultTable.Rows[0]["OrderCount"];

                //  RtnString =  OrderCount.ToString();
                RtnString = ResultTable.Rows[0]["CustomerCount"].ToString();
            }
            else
            {
                RtnString = "0";
            }
            return RtnString;


        }
        public DataTable ItemList() //List of Items 
        {
            string _vItemStatus = "Publish";
            string SQL_str = "SELECT " +
                              "itemID" +
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
        public DataTable ItemOnStockList() //List of Stock Items 
        {
            string _vItemStatus = "Publish";
            string SQL_str = "SELECT " +
                              "itemID" + '"' + "Item #" + '"' +

                              ",vItemDesc1" + '"' + "Description" + '"' +

                              ",vItemPrice" + '"' + "Price" + '"' +
                              ",vItemVendor" + '"' + "Vendor" + '"' +
                              ",vItemVendorContact" + '"' + "Contact Name" + '"' +
                              ",vItemVendorPhone" + '"' + "Phone" + '"' +
                              ",POS_ItemCategoryId" + '"' + "Category" + '"' +
                          " FROM POS_ItemMaster" + " where vItemStatus='" + _vItemStatus + "'";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POSitemMaster");
            ResultTable = ResultSet.Tables[0];

            return ResultTable;


        }
        public DataTable PriceList() //List of Items 
        {
            string _vItemStatus = "Publish";
            string SQL_str = "SELECT " +
                              "itemID" + '"' + "Item #" + '"' +
                              ",vItemDesc1" + '"' + "Description " + '"' +
                              ",vItemPrice" + '"' + "Price" + '"' +
                              ",vItemAvailability" + '"' + "Avail" + '"' +
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
        public DataTable CustomerList() //List of Customers 
        {

            string SQL_str = "SELECT " +
                            "iCustid" +
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
                              " FROM POS_Customer"; // + " where vCustStatus='" + _vCustStatus + "'";


            //Connect to database and retrieve
            SqlCEDataAccess POSdbAccess = new SqlCEDataAccess();

            DataSet ResultSet;
            DataTable ResultTable;

            ResultSet = POSdbAccess.dsGetData(SQL_str, "POScustomer");
            ResultTable = ResultSet.Tables[0];

            return ResultTable;


        }
    }
}
