using BaseAppData.Concrete;
using BaseAppData.Entity;
using BaseAppUI.Configuration.Sync;
using BaseAppServerCom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseAppUI.ViewModel.Sections.Partial;
using BaseAppUI.Model.Card;

namespace BaseAppUI.Configuration
{
   public static class GConfig
    {
       public static Synchronizer Synchronizer;
       public static WooCommerceClient WCClient;
       public static DALUnitOfWork DAL;
        public static string POSuser; //Added by SAA for flow doc receipt. 4/24/16
        public static string POSuserAccess="ADMIN"; //Added by SAA for User access 6/03/16
        public static POS_Setup POS_Setup = null;

        public static decimal total_amount;
        public static OrderVM GCorder;
        public static bool SyncRequest;
        public static string SyncLog;

        public static SaleResponse response2;

        public static bool ShouldKitchenCancel = false;


        public static void Initialize()
        {
          
            Synchronizer = new Synchronizer();

            string ConsumerKey = BaseAppUI.Properties.Settings.Default.CKey;
            string ConsumerSecret = BaseAppUI.Properties.Settings.Default.CSecret;
            string storeUrl = BaseAppUI.Properties.Settings.Default.Url;


            //// from config file
            //string ConsumerKey = "ck_b7a72263b6bf95a345c87951f04810a120826ba9";
            //string ConsumerSecret = "cs_8e982aafabc481195e62c88f4685ea6a5fd99290";
            //string storeUrl = "http://pos.americloud.net/tastynihari/";


            //from config file
            //string ConsumerKey = "ck_4d91f552cad6f97e95cf6e61d74316dbb27ef3d6";
            //string ConsumerSecret = "cs_2ecd7c5e5f0a46958a1af4746f0da36bc82e070a";
            //string storeUrl = "http://www.pitaandkabobz.com";


            WCClient = new WooCommerceClient(storeUrl, ConsumerKey, ConsumerSecret);
            DAL = new DALUnitOfWork();

            PrepareCompany();

            PrepareGuestUser();
        }

        private static void PrepareCompany()
        {
            //will create or update company info from config file

                POS_Setup = DAL.POS_Setups.All().FirstOrDefault();

                if (POS_Setup == null)
                {

                   POS_Setup = new POS_Setup { vCorpName = BaseAppUI.Properties.Settings.Default.BusinessName, vStatus = "testing",vSalesTax=0 };

                   DAL.POS_Setups.Create(POS_Setup);
                   DAL.SaveChanges();

                 

                }
                else
                {
                    //update info

                }

        }


        private static void PrepareGuestUser()
        {


            if (!GConfig.POS_Setup.Customers.Any(n => n.vCustContactNme == "Guest Customer"))
            {
                //create guest user
                POS_Customer custo = new POS_Customer() { 
                
                    vCustContactNme="Guest Customer"
                };

                GConfig.POS_Setup.Customers.Add(custo);
                GConfig.DAL.SaveChanges();
            }
        }

        public static int GenerateLocalOrderNumber()
        {


              int latest=POS_Setup.OrderHeaders.Any() ? POS_Setup.OrderHeaders.Max(n => n.vOrderNumber): 0;

              latest++;

              if (POS_Setup.OrderHeaders.Any(n => n.vOrderNumber == latest))
              {
                return  GenerateLocalOrderNumber();
              }
              else return latest;

           
        }
       
    }
}
