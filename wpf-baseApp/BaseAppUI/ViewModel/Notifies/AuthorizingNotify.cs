using BaseAppUI.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BaseAppUI.ViewModel.Notifies
{
   public class AuthorizingNotify:NotifyBase
    {

       public AuthorizingNotify(Func<ISaleModel,bool> authorize,ISaleModel ccsale)
           : base()
       {

           authorize.BeginInvoke(ccsale,AuthorizeCallBack,authorize);

       }

       private void AuthorizeCallBack(IAsyncResult ar)
       {
           Func<ISaleModel,bool> function = ar.AsyncState as Func<ISaleModel, bool>;
           bool result = function.EndInvoke(ar);

         


          
         
       }
    }
}
