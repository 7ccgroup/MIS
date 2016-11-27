using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BaseAppServerCom.Helpers;
using BaseAppServerCom.Models;
using BaseAppServerCom.Models.Orders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestApiCalls
{
    class Program
    {
        static void Main(string[] args)
        {

            string ConsumerKey = "ck_4d91f552cad6f97e95cf6e61d74316dbb27ef3d6";
            string ConsumerSecret = "cs_2ecd7c5e5f0a46958a1af4746f0da36bc82e070a";
            string storeUrl = "http://www.pitaandkabobz.com";

            BaseAppServerCom.WooCommerceClient client = new BaseAppServerCom.WooCommerceClient(storeUrl, ConsumerKey, ConsumerSecret);


           


         
           // var cust = new Customer() { first_name="update works", last_name = "yurt",email="yurthakan55@gmail.com" };

           ////var res=  client.CreateCustomer(cust).Result;
           // var s = client.CustomerService.Update(22,cust).Result;


            //order id: 1407
           //var orders= client.OrderService.All().Result;

            //var order = client.OrderService.GetById(1414).Result;


           

            ////order.note = "order update works";
            //order.status = "test";
          

        

           //var neworder= client.OrderService.Update(1414,order).Result;


            //order meta
            var neworder = new Order
            {
                order_meta = new Dictionary<string, string>()
            };

            neworder.order_meta[Order.OrderTypeKey] = "Dine-In";
            var createdorder = client.OrderService.Create(neworder).Result;


         




        }
            


       
    }
}
