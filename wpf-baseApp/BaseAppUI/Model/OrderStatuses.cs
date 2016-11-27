using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.Model
{
    public struct OrderStatuses
    {
        #region fields
        static string _processing = "Processing";
        static string _pending = "Pending";
        static string _onhold = "On-Hold";
        static string _completed = "Completed";
        static string _cancelled = "Cancelled";
        static string _refunded = "Refunded";
        static string _failed = "Failed";
        #endregion

        public static string Processing
        {

            get
            {
                return _processing;
            }
        }
        public static string Pending
        {

            get
            {
                return _pending;
            }
        }
        public static string OnHold
        {

            get
            {
                return _onhold;
            }
        }
        public static string Completed
        {

            get
            {
                return _completed;
            }
        }
        public static string Cancelled
        {

            get
            {
                return _cancelled;
            }
        }
        public static string Refunded
        {

            get
            {
                return _refunded;
            }
        }

        public static string Failed
        {

            get
            {
                return _failed;
            }
        } 
      

    }
}
