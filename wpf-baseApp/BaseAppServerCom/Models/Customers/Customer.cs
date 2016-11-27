using BaseAppServerCom.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom.Models
{
   public class Customer
    {
       public int id { get; set; }
       public string email { get; set; }
       public string first_name { get; set; }
       public string last_name { get; set; }
       public BillingAddress billing_address { get; set; }
       public ShippingAddress shipping_address { get; set; }
    }
}
