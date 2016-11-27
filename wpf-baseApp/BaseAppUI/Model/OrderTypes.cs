using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.Model
{
    public struct OrderTypes
    {
        static string _online = "Online";
        static string _dine_in = "Dine-In";
        static string _to_go = "To-Go";
        static string _delivery = "Delivery";
        static string _catering = "Catering";
        public static string Online
        {
            get { return _online; }
        }
        public static string DineIn
        {
            get { return _dine_in; }
        }
        public static string ToGo
        {
            get { return _to_go; }
        }
        public static string Delivery
        {
            get { return _delivery; }
        }
        public static string Catering
        {
            get { return _catering; }
        }

        public static string None
        {
            get
            {
                return null;
            }
        }
   
    }
  
}
