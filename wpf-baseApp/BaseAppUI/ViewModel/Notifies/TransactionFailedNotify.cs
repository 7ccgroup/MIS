using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.ViewModel.Notifies
{
   public class TransactionFailedNotify:NotifyBase
    {
       public string ErrorCode { get; set; }
       public string ErrorMessage { get; set; }
       //private DelegateCommand _retryCommand;
       //Action _retry;

       public TransactionFailedNotify(string errorcode,string errormessage,Action actionretry=null)
       {
           //_retry = actionretry;
           ErrorCode = errorcode;
           ErrorMessage = errormessage;

       }


       //public DelegateCommand RetryCommand
       //{
       //    get { return _retryCommand ?? (_retryCommand = new DelegateCommand(() => {

       //        if (_retry != null)
       //        {
       //            _retry.Invoke();
       //        }

       //    })); }
       //}
    }
}
