using ccSale;
using System;

namespace BaseAppUI.Model.Card
{
    class SaleGenericSwipedModel : ISaleModel
    {
        string _amount;
        string _trackdata;

        public void Set(string amount, string trackdata)
        {
            _amount = amount;
            _trackdata = trackdata;
        }

        public SaleResponse Process()
        {
            ccSaleGenericSwiped newsale = new ccSaleGenericSwiped();
            newsale.setAmount(_amount);

            try
            {
                newsale.Process(_trackdata);
            }

            catch(Exception ex)
            {
                var error = ex.Message.ToString();
                return new SaleResponse { ErrorMessage = ex.HResult.ToString() };
            }

            return new SaleResponse
            {
                Resp_Msg = newsale.Resp_Msg,
                Resp_ApprovalCode = newsale.Resp_ApprovalCode,
                Resp_CardNum = newsale.Resp_CardNum,
                Resp_TxnId = newsale.Resp_TxnId,
                ErrorMessage = newsale.Resp_Msg,
            };
        }
    }
}
