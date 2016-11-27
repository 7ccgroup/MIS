using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom.Models
{
   public class CustomersBundle
    {
        [JsonProperty("customers")]
      public IEnumerable<Customer> Content { get; set; }
    }
}
