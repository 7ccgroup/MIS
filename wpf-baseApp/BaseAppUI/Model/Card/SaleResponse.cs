using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppUI.Model.Card
{

  
    public class SaleResponse
    {
        public string Resp_CardNum { get; set; }
        public string Resp_ApprovalCode { get; set; }
        public string Resp_TxnId { get; set; }
        public string Resp_Msg { get; set; }

        public string ErrorMessage { get; set; }


        public bool IsValid
        {
            get
            {
                return Resp_Msg == "APPROVAL";
            }
        }
    }
}
