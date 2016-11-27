using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ccSale
{
    public class ccVoid
    {
        string ssl_merchant_id = BaseAppUI.Properties.Settings.Default.MerchantID;
        string ssl_user_id = BaseAppUI.Properties.Settings.Default.UserID;
        string ssl_pin = BaseAppUI.Properties.Settings.Default.Pin;
        string ssl_result_format = "ascii";
        string ssl_transaction_type = "ccvoid";
        // Note: ssl_show_form does not apply on Void transactions

        string ssl_txn_id;

        string Response;
        string Response2;

        public string Resp_TxnTime;
        public string Resp_Msg;
        public string Resp_TxnId;
        public string Resp_ApprovalCode;
        public string Resp_ErrorCode;

        public void SetVoidDetails(string TxnID)
        {
            ssl_txn_id = HttpUtility.UrlEncode(TxnID);
        }

        public void Process()
        {
            string PostData = "ssl_merchant_id=" + ssl_merchant_id +
                                   "&ssl_user_id=" + ssl_user_id +
                                   "&ssl_pin=" + ssl_pin +
                                   "&ssl_transaction_type=" + ssl_transaction_type +
                                   "&ssl_result_format=" + ssl_result_format +
                                   "&ssl_txn_id=" + ssl_txn_id;

            string url = "https://www.myvirtualmerchant.com/VirtualMerchant/process.do";

            // Create a new web request
            HttpWebRequest GatewayRequest = (HttpWebRequest)WebRequest.Create(url);

            // Set HTTP header information
            GatewayRequest.Method = "POST";
            GatewayRequest.ContentType = "application/x-www-form-urlencoded";

            byte[] byteArray = Encoding.UTF8.GetBytes(PostData);
            GatewayRequest.ContentLength = byteArray.Length;

            // Send request	
            Stream SendStream = GatewayRequest.GetRequestStream();
            SendStream.Write(byteArray, 0, byteArray.Length);
            SendStream.Close();

            // Get response
            HttpWebResponse GatewayResponse = (HttpWebResponse)GatewayRequest.GetResponse();
            Stream ReceiveStream = GatewayResponse.GetResponseStream();
            StreamReader ReadStream = new StreamReader(ReceiveStream, Encoding.UTF8);
            Response = ReadStream.ReadToEnd();
            Response2 = Response;

            // Close resources
            GatewayResponse.Close();
            ReadStream.Close();

            // Parse the response and store
            // individual elements
            var dict = Response2.Split(
                        new[] { '\n' }).Select(part => part.Split('=')).ToDictionary
                        (split => split[0], split => split[1]);

            //Error Handling Statements

            if (dict.TryGetValue("errorName", out Resp_Msg))
            {
                Resp_ErrorCode = dict["errorCode"];
            }

            if (dict.ContainsKey("ssl_result_message"))
            {
                Resp_Msg = dict["ssl_result_message"];   // Look for "APPROVAL"          

                // Transaction identifier of the successfull 
                // refund transaction
                Resp_TxnId = dict["ssl_txn_id"];
                Resp_TxnTime = dict["ssl_txn_time"];
                Resp_ApprovalCode = dict["ssl_approval_code"];
            }
        }
    }
}
