//DLL v1.2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Web;
using System.Net;
using System.IO;

namespace ccSale
{
    public class ccSaleKeyed
    {
        string _elapsed;

        string ssl_merchant_id = BaseAppUI.Properties.Settings.Default.MerchantID;
        string ssl_user_id = BaseAppUI.Properties.Settings.Default.UserID;
        string ssl_pin = BaseAppUI.Properties.Settings.Default.Pin;
        string ssl_show_form = "false";
        string ssl_result_format = "ascii";
        string ssl_transaction_type = "CCSALE";
        string ssl_amount;
        string ssl_card_number;
        string ssl_exp_date;
        string ssl_cvv2cvc2;


        string Response1;
        string Response2;
        public string Resp_RecvdTime;
        public string Resp_CardNum;
        public string Resp_Msg;
        public string Resp_ApprovalCode;
        public string Resp_ErrorCode;
        public string Resp_TxnId;

        public string Elapsed
        {
            get
            {
                return _elapsed;
            }

        }
        public void Process(string CNum, string exp, string cvv, string Amt)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Set Required Sale Fields
            ssl_card_number = CNum;
            ssl_exp_date = exp;
            ssl_cvv2cvc2 = cvv;
            ssl_amount = Amt;

            string PostData = "ssl_merchant_id=" + ssl_merchant_id +
                                   "&ssl_user_id=" + ssl_user_id +
                                   "&ssl_pin=" + ssl_pin +
                                   "&ssl_transaction_type=" + ssl_transaction_type +
                                   "&ssl_show_form=" + ssl_show_form +
                                   "&ssl_result_format=" + ssl_result_format +
                                   "&ssl_amount=" + ssl_amount +
                                   "&ssl_card_number=" + ssl_card_number +
                                   "&ssl_exp_date=" + ssl_exp_date +
                                   "&ssl_cvv2cvc2=" + ssl_cvv2cvc2;

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
            Response1 = ReadStream.ReadToEnd();
            Response2 = Response1;
            Resp_RecvdTime = DateTime.Now.ToString();

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            _elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

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
                Resp_Msg = dict["ssl_result_message"];
                Resp_CardNum = dict["ssl_card_number"];
                Resp_ApprovalCode = dict["ssl_approval_code"];
                Resp_TxnId = dict["ssl_txn_id"];
            }
        }

    }

}