using ccSale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.Model.Card
{

    public class SaleKeyedModel : ISaleModel
    {
        string _cnum;
        string _exp;
        string _cvv;
        string _amt;

        public void Set(string CNum, string exp, string cvv, string Amt)
        {
            _cnum = CNum;
            _exp = exp;
            _cvv = cvv;
            _amt = Amt.Replace(",","."); 

        }

        public SaleResponse Process()
        {
            ccSaleKeyed newsale = new ccSaleKeyed();
            try
            {
                newsale.Process(_cnum, _exp, _cvv, _amt);
            }
            catch(Exception ex)
            {
                var error = ex.Message.ToString();
            }
            return new SaleResponse
            {
                Resp_Msg = newsale.Resp_Msg,
                Resp_ApprovalCode = newsale.Resp_ApprovalCode,
                Resp_CardNum = newsale.Resp_CardNum,
                Resp_TxnId = newsale.Resp_TxnId,
                ErrorMessage=newsale.Resp_Msg
            };


            

  
        }

       
    }
}
