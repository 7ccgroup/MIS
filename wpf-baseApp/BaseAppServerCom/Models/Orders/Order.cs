
using Newtonsoft.Json;
using BaseAppServerCom.Models.Customers;
using BaseAppServerCom.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom.Models
{

    public class PaymentDetails {

        public string method_id { get; set; }
        public string method_title { get; set; }
        public bool paid { get; set; }
    }

   

    

    public class ShippingItem { 
    
    }
    public class TaxItem
    {
        public int id { get; set; }
        public string rate_id { get; set; }
        public string code { get; set; }
        public string title {get; set;}
        public double total { get; set; }
        public bool compound { get; set; }
    }
    public class FeeItem
    {


    }

    public class CouponItem
    {
        public int id { get; set; }
        public string code { get; set; }
        public double amount { get; set; }

    }

    public class OrderItem {

        public int id { get; set; }
        public double subtotal { get; set; }
        public double subtotal_tax { get; set; }
        public double total { get; set; }
        public double total_tax { get; set; }

        public double price { get; set; }
        public int quantity { get; set; }

        public object tax_class { get; set; }
        public string name { get; set; }

        public int? product_id { get; set; }
        public string sku { get; set; }
        public List<object> meta { get; set; }

    }
    public class Order
    {

        public int id { get; set; }
        public string order_number { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime completed_at { get; set; }
        public string status { get; set; }
        public string currency { get; set; }
        public double total { get; set; }
        public double subtotal { get; set; }

        public int total_line_items_quantity { get; set; }
        public double total_tax { get; set; }
        public double total_shipping { get; set; }
        public double cart_tax { get; set; }
        public double shipping_tax { get; set; }

        public double total_discount { get; set; }

        public string shipping_methods { get; set; }

        public PaymentDetails payment_details { get; set; }
        public BillingAddress billing_address { get; set; }
        public ShippingAddress shipping_address { get; set; }
        public string customer_ip { get; set; }
        public string customer_user_agent { get; set; }
        public int customer_id { get; set; }
        public string view_order_url { get; set; }

        public List<OrderItem> line_items { get; set; }
        public List<ShippingItem> shipping_lines { get; set; }
        public List<TaxItem> tax_lines { get; set; }
        public List<FeeItem> fee_lines { get; set; }
        public List<CouponItem> coupon_lines { get; set; }

        public Customer customer { get; set; }

        public string order_type { get; set; }

        public string note { get; set; }

        //private OrderCustomProperties _orderCustomProperties;

        //public OrderMeta OrderCustomProperties
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(note)) return null;

        //        var obj = JsonConvert.DeserializeObject<OrderMeta>(note);
        //        return obj;
        //    }
        //    set
        //    {

        //        note = JsonConvert.SerializeObject(value);


        //    }
        //}

        public Dictionary<string,string> order_meta { get; set; }


        public static string OrderTypeKey = "wc_pos_order_type";

    }
    
}
