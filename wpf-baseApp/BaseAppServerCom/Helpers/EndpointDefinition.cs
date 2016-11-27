using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom.Helpers
{
   
   public  class EndpointDefinition
    {
        static string Products = "products";
        static string Orders = "orders";
        static string Categories="products/categories";

       public static string GetValueByEndpoint(Endpoint endpoint)
       {
           switch (endpoint)
           {
               case Endpoint.Products: return Products;
               case Endpoint.Categories: return Categories;
               case Endpoint.Orders: return Orders;
               default: return null ;

           }

       }
 
    }
}
