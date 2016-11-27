using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseAppUI.Configuration;
using BaseAppData.Entity;
using BaseAppServerCom.Models;
using System.Threading;
using BaseAppUI.Model;

namespace BaseAppUI.Configuration.Sync
{
   public class Synchronizer
    {
       public event Action<SyncState> SyncStateChangeEvent;
       public event Action<Category, SyncType> SyncItemCategoryChangeEvent;
       public event Action<Product, SyncType> SyncItemMasterChangeEvent;
       public event Action<decimal> SyncTaxRateChangeEvent;


       CancellationTokenSource tokensource = new CancellationTokenSource();

       private void OnSyncStateChangeAction(SyncState syncstate)
       {
           if (SyncStateChangeEvent != null)
               SyncStateChangeEvent(syncstate);
       }
       private void OnSyncItemCategoryChangeAction(Category item, SyncType type)
       {
           if (SyncItemCategoryChangeEvent != null)
               SyncItemCategoryChangeEvent(item,type);
       }
       private void OnSyncItemMasterChangeAction(Product item, SyncType type)
       {
           if (SyncItemMasterChangeEvent != null)
               SyncItemMasterChangeEvent(item, type);
       }
       private void OnSyncTaxRateChangeAction(decimal rate)
       {
           if (SyncTaxRateChangeEvent != null)
               SyncTaxRateChangeEvent(rate);
       }

       private SyncState state;
       public void StartSync()
       {
           if (state == SyncState.Start) return;

           state = SyncState.Start;
           OnSyncStateChangeAction(state);

           Task.Factory.StartNew(() =>
           {
              
               startsyncQueue();

           },tokensource.Token).ContinueWith(a => {

               state = SyncState.End;
               OnSyncStateChangeAction(state);

           });

          
           
       }


       private void startsyncQueue(){
           if (BaseAppUI.Properties.Settings.Default.SyncMode == "Online" || GConfig.SyncRequest==true) //added by SAA on 3/29/16
           {
                if (GConfig.POS_Setup.ItemCategories.Any() == false || GConfig.SyncRequest==true)
                {
                    syncAllCategories(); //Get All Categories at once ZC 6/23
                    syncAllProducts();    //Get All Products at once ZC 6/22
                }
                else
                {

                    syncCategories();
                    syncItemMasters();
                }
                syncTax();
                Console.WriteLine("Sync Completed");
            }
       }


       private void syncTax()
       {
           var localvalue = GConfig.POS_Setup.vSalesTax;
            GConfig.SyncLog += "\n"+" Sync Tax"+" Thread : " +Thread.CurrentThread.ManagedThreadId.ToString()+"\n";
            try {
                var result = GConfig.WCClient.GetTaxes().Result;

            if (result!=null){
               var remotevalue = result.FirstOrDefault(n => n.@class == "standard");
                if (remotevalue != null && localvalue != remotevalue.rate)
                   OnSyncTaxRateChangeAction(remotevalue.rate);
           }

            }
            catch
            {

            }




        }

       private void syncCategories()
       {
         
           //remote cats
           var remoteIds = GConfig.WCClient.GetIDs(BaseAppServerCom.Helpers.Endpoint.Categories);
            Console.WriteLine("Sync Categories");
           //local cats
           IList<int> _localIds = GConfig.POS_Setup.ItemCategories.Select(n => n.CatID).ToList();
            try {
                IList<int> _remoteIds = remoteIds.Result;

                IList<int> _newcatIds = _remoteIds.Where(n => !_localIds.Any(m => m == n)).Select(n => n).ToList();


                if (_newcatIds != null && _newcatIds.Count > 0)
                {
                    foreach (var id in _newcatIds)
                    {
                        var rcat = GConfig.WCClient.GetCategoryById(id).Result;
                        Console.WriteLine("Cat ID:" + rcat.id + " Category:" + rcat.name); //SAA 7/12/16

                        OnSyncItemCategoryChangeAction(rcat, SyncType.Add);
                    }

                }




            }
            catch
            { }
          
          

       }

       private void syncItemMasters()
       {

           //remote itemss
           var remoteIds = GConfig.WCClient.GetIDs(BaseAppServerCom.Helpers.Endpoint.Products);
            Console.WriteLine("Sync ItemMaster");
           //local items
           IList<int> _localIds = GConfig.POS_Setup.ItemCategories.SelectMany(n=>n.POS_ItemMasters.Select(m=>m.itemID)).ToList();
            try {
                IList<int> _remoteIds = remoteIds.Result;

                IList<int> _newitemIds = _remoteIds.Where(n => !_localIds.Any(m => m == n)).Select(n => n).ToList();


                if (_newitemIds != null && _newitemIds.Count > 0)
                {
                    foreach (var id in _newitemIds)
                    {
                        var product = GConfig.WCClient.GetProductById(id).Result;
                        Console.WriteLine("Item ID" + product.id + " Item:" + product.title + " Category: " + product.categories.ElementAtOrDefault(0) + " Price:" + product.price); //SAA 7/12/16

                        if (product != null)
                            OnSyncItemMasterChangeAction(product, SyncType.Add);
                    }

                }



            }
            catch
            { }




       }

        private void syncAllCategories() //Get all categories ZC 6/23
        {
          //  var cat = GConfig.POS_Setup.ItemCategories.FirstOrDefault(n => n.vCategoryCode == "custom-cat");
            var catAll = GConfig.POS_Setup.ItemCategories.FirstOrDefault(n => n.vCategoryShortDesc != "custom-cat");

            Console.WriteLine("Sync All Categories");
            var allCategories = GConfig.WCClient.GetCategories().Result;
            if (allCategories != null) //Check if API call returned anything ZC 8/11
            {
                for (int index = 0; index < allCategories.Count; index++)
                {

                    var product = allCategories.ElementAt(index);
                    if (catAll != null)
                    {
                        var CheckCatIndb = catAll.POS_Setup.ItemCategories.Where(n => n.CatID == product.id);

                        if (CheckCatIndb == null)
                        {
                            Console.WriteLine("Cat ID: " + product.id + " Category: " + product.name); //SAA 7/12/16
                            OnSyncItemCategoryChangeAction(product, SyncType.Add);
                        }
                        else
                            Console.WriteLine("Cat ID: " + product.id + " Category: " + product.name + "  ==> Exists in db"); //SAA 7/12/16
                    }
                    else //if it is empty db do this....
                    {
                        Console.WriteLine("Cat ID: " + product.id + " Category: " + product.name); //SAA 7/12/16
                        OnSyncItemCategoryChangeAction(product, SyncType.Add);
                    }

                }
                Console.WriteLine("Category Count: " + allCategories.Count + "\n");
            }
            else
            {
                Console.WriteLine("Connection Error");
            }
        }
        private void syncAllProducts() //Get all products ZC 6/22
        {
            ItemsBuilder Items = new ItemsBuilder(); //SAA 7/13

            Console.WriteLine("Sync All Products");
            var allLocalProducts = Items.ItemListDisplayAdvance();
            var allProducts = GConfig.WCClient.GetProducts().Result;
            if (allProducts != null) //Check if API call returned anything ZC 8/11
            {
                for (int index = 0; index < allProducts.Count; index++)
                {
                    var product = allProducts.ElementAt(index);
                    if (product.categories.ElementAtOrDefault(0) != null)
                    {
                        var CheckCatIndb = allLocalProducts.FirstOrDefault(n => n.itemID == product.id);
                        if (CheckCatIndb == null)
                        {
                            Console.WriteLine("Item ID " + product.id + " Item: " + product.title + " Category: " + product.categories.ElementAtOrDefault(0) + " Price: " + product.price); //SAA 7/12/16
                            OnSyncItemMasterChangeAction(product, SyncType.Add);
                        }
                        else
                            Console.WriteLine("Item ID " + product.id + " Item: " + product.title + " Category: " + product.categories.ElementAtOrDefault(0) + " Price: " + product.price + "  ==> Exists in db"); //SAA 7/12/16
                    }
                    else
                        Console.WriteLine("Item ID " + product.id + " Item: " + product.title + " Category: " + product.categories.ElementAtOrDefault(0) + " Price: " + product.price + " Item with wrong category"); //SAA 7/12/16

                }
                Console.WriteLine("Product Count: " + allProducts.Count + "\n");
            }
            else
            {
                Console.WriteLine("Connection Error");
            }
        }
    }
}
