using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.Model
{
    public struct OrderPaymentTypes
    {
        static string _cash = "Cash";
        static string _card="Credit Card";
        static string _split = "Split";

        public static string None
        {
            get
            {
                return null;
            }
        }
        public static string Cash
        {
            get
            {
                return _cash;
            }
        }
        public static string Card
        {
            get
            {
                return _card;
            }
        }
        public static string Split
        {
            get
            {
                return _split;
            }
        }
    }
}
