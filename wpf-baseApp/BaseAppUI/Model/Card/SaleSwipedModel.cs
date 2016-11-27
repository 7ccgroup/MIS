using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.Model.Card
{
    public class SaleSwipedModel : ISaleModel
    {
        string _amount;
        Microsoft.PointOfService.Msr _msr;
        public void Set(string amount, Microsoft.PointOfService.Msr msr)
        {
            _amount = amount.Replace(",",".");
            _msr = msr;
        }
        public SaleResponse Process()
        {
            ccSale.ccSaleSwiped newsale = new ccSale.ccSaleSwiped();
            newsale.setAmount(_amount);

            try { 
            newsale.Process(_msr);
                }
            catch(Exception ex) {

                var error = ex.Message.ToString();

                return new SaleResponse {ErrorMessage=ex.HResult.ToString() };
            }

            return new SaleResponse
            {
                Resp_Msg = newsale.Resp_Msg,
                Resp_ApprovalCode = newsale.Resp_ApprovalCode,
                Resp_CardNum = newsale.Resp_CardNum,
                Resp_TxnId = newsale.Resp_TxnId,
                ErrorMessage=newsale.Resp_Msg,
            };
        }

       
    }
}
