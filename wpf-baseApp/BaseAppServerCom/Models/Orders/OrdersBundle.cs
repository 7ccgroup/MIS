using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom.Models.Orders
{
    internal class OrdersBundle
    {
        [JsonProperty("orders")]
        public IEnumerable<Order> Content { get; set; }
    }
}
