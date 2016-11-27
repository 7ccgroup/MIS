using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.Model
{
    public struct ReportTypes
    {
        static string _sales = "Sales";
        static string _customers = "Customers";
        static string _items = "Items";
        static string _pricing = "Pricing";
        static string _inventory = "Inventory";
        static string _tips = "Tips";
        static string _transactionList = "TransactionsList";
        static string _dashboard = "Dashboard";
        static string _settleAll = "Settle All";
        static string _trends = "Trends";


        public static string Sales
        {
            get { return _sales; }
        }
        public static string Customers
        {
            get { return _customers; }
        }
        public static string Items
        {
            get { return _items; }
        }
        public static string Pricing
        {
            get { return _pricing; }
        }
        public static string Inventory
        {
            get { return _inventory; }
        }
        public static string TransactionList
        {
            get { return _transactionList; }
        }
        public static string Tips
        {
            get { return _tips; }
        }

        public static string Dashboard
        {
            get { return _dashboard; }
        }
        public static string Trends
        {
            get { return _trends; }
        }
        public static string SettleAll
        {
            get { return _settleAll; }
        }

        public static string None
        {
            get
            {
                return null;
            }
        }

    }

}
