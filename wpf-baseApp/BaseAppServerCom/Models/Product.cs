using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseAppServerCom.Models
{
   public class Product
    {
       public int id { get; set; }
        public string title { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

       //image
        public string featured_src { get; set; }

        public string type { get; set; }
        public string status { get; set; }
        public double price { get; set; }
        public double? regular_price { get; set; }

       public double?  sale_price{get;set;}
        public bool taxable { get; set; }
        public string tax_status { get; set; }
        public string tax_class { get; set; }

        public bool managing_stock { get; set; }
        public int? stock_quantity { get; set; }
        public bool in_stock { get; set; }

        public bool backorders_allowed { get; set; }
        public bool backordered { get; set; }

        public bool sold_individually { get; set; }
        public bool purchaseable { get; set; }

        public bool featured { get; set; }

        public bool visible { get; set; }

        public bool on_sale { get; set; }

        public uint total_sales { get; set; }

        public IList<string> categories { get; set; }

        public string description { get; set; }

        public bool shipping_required { get; set; }
        public bool shipping_taxable { get; set; }

    }
}
