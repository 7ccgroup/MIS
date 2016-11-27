using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ConvergeAPI
{
    public class Settle
    {
        string ssl_merchant_id = BaseAppUI.Properties.Settings.Default.MerchantID;
        string ssl_user_id = BaseAppUI.Properties.Settings.Default.UserID;
        string ssl_pin = BaseAppUI.Properties.Settings.Default.Pin;
        string ssl_result_format = "ascii";
        string ssl_transaction_type = "settle";
        // Note: ssl_show_form does not apply on Settle transactions

        string ssl_txn_id = " ";

        string Response;
        string Response2;

        public string Resp_TxnTime;
        public string Resp_Msg;
        public string Resp_TxnId;
        public string Resp_BatchCount;
        public string Resp_BatchAmt;
        public string Resp_ErrorCode;

        public void SettleIndividual(string TxnID)
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

            dict.TryGetValue("ssl_result_message", out Resp_Msg);
            dict.TryGetValue("ssl_txn_time", out Resp_TxnTime);// Look for "Scheduled for Settlement" 
            dict.TryGetValue("ssl_txn_id", out Resp_TxnId);
            dict.TryGetValue("ssl_txn_main_count", out Resp_BatchCount);
            dict.TryGetValue("ssl_txn_main_amount", out Resp_BatchAmt);

            // Set ssl_txn_id back to just an empty space
            ssl_txn_id = " ";

        }
    }
}
