using BaseAppServerCom.Models;
using BaseAppServerCom.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom.Services
{
   public class OrderService:Service
    {
       public OrderService(WooCommerceClient client) : base(client) { }

       public async Task<Order> GetById(int orderId)
       {
           //filter[meta]=true
           //Dictionary<string, string> parameters = new Dictionary<string, string>();
           //parameters.Add("filter[meta]", "true");


           var endPoint = string.Format("orders/{0}", orderId);
           return (await Get<OrderBundle>(endPoint)).Content;
       }
       public async Task<IEnumerable<Order>> All(Dictionary<string, string> parameters = null)
       {
           if (parameters == null)
               parameters = new Dictionary<string, string>();
           
           parameters["filter[limit]"] = "-1";

           return (await Get<OrdersBundle>(apiEndpoint: "orders", parameters: parameters)).Content;
       }

       public async Task<Order> Create(Order data)
       {
           var bundle = new OrderBundle { Content = data };
           return (await Post(apiEndpoint: "orders", toSerialize: bundle)).Content;
       }

       public async Task<Order> Update(int orderId, Order newData)
       {
           var endPoint = String.Format("orders/{0}", orderId);
           var bundle = new OrderBundle { Content = newData };
           return (await Put(endPoint, toSerialize: bundle)).Content;
       }
    }
}
