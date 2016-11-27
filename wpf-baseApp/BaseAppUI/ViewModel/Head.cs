using BaseAppUI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.ViewModel
{
   public class Head:NotifyProperty
    {
        private bool _isSync;

        public bool IsSync
        {
            get { return _isSync; }
            set { _isSync = value;
            OnPropertyChanged("IsSync");
            }
        }

        public string CompanyName
        {
            get { return BaseAppUI.Properties.Settings.Default.BusinessName + " - CloudPOS"; } // Pull store name from settings - Amjad 5/19
        }

        public string _useraccess;
        public string UserAccess
        {
            get {
                return _useraccess;
            }
            set {                    _useraccess =value;
                OnPropertyChanged("UserAccess"); }
        }
    }
}
