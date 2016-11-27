using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom.Models
{
    internal class CustomerBundle
    {
        [JsonProperty("customer")]
        public Customer Content { get; set; }
    }
}
