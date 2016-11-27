using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom.Models
{
   public class Tax
    {
        public string id {get;set;}
     public string country{get;set;}
     public string state{get;set;}
       public string postcode{get;set;}
      public string city {get;set;}
      public decimal rate{get;set;}
      public string name{get;set;}
       public int priority{get;set;}
       public bool compound{get;set;}
       public bool shipping{get;set;}
       public int order{get;set;}
       public string @class {get;set;}

     
    }
}
