using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using BaseAppServerCom.Models;
using Newtonsoft.Json;
using BaseAppServerCom.Services;

namespace BaseAppServerCom
{
    public partial class WooCommerceClient
    {
      
        HttpClient httpClient;

        private const string API_ENDPOINT = "wc-api/v3/";
        private string StoreUrl { get; set; }

        private string ConsumerKey { get; set; }
        private string ConsumerSecret { get; set; }
        private bool IsSsl { get;set; }


        private CustomerService _customerService;
        private OrderService _orderService;


        public CustomerService CustomerService
        {
            get { return _customerService ?? (_customerService = new CustomerService(this)); }
        }

        public OrderService OrderService
        {
            get { return _orderService ?? (_orderService = new OrderService(this)); }
        }

        

        public WooCommerceClient(string storeUrl, string consumerKey, string consumerSecret,bool isssl=false)
        {
            

          

            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            this.StoreUrl = storeUrl.TrimEnd('/') + "/" + API_ENDPOINT;
            this.IsSsl = isssl;
           
        }

        private static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }

        private string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
        private string GenerateSignature(Dictionary<string, string> parameters, string method, string endpoint)
        {
            var baserequesturi = Regex.Replace(HttpUtility.UrlEncode(this.StoreUrl + endpoint), "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
            var normalized = NormalizeParameters(parameters);

            var signingstring = string.Format("{0}&{1}&{2}", method, baserequesturi,
                string.Join("%26", normalized.OrderBy(x => x.Key).ToList().ConvertAll(x => x.Key + "%3D" + x.Value)));

            //valid for v1 and v2
            //var signature =Convert.ToBase64String(HashHMAC(Encoding.UTF8.GetBytes(this.ConsumerSecret),Encoding.UTF8.GetBytes(signingstring)));

            //valid for v3
            var signature = Convert.ToBase64String(HashHMAC(Encoding.UTF8.GetBytes(this.ConsumerSecret + "&"), Encoding.UTF8.GetBytes(signingstring)));

            return signature;
        }

        private Dictionary<string, string> NormalizeParameters(Dictionary<string, string> parameters)
        {
            var result = new Dictionary<string, string>();
            foreach (var pair in parameters)
            {
                var key = HttpUtility.UrlEncode(HttpUtility.UrlDecode(pair.Key));
                key = Regex.Replace(key, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper()).Replace("%", "%25");
                var value = HttpUtility.UrlEncode(HttpUtility.UrlDecode(pair.Value));
                value = Regex.Replace(value, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper()).Replace("%", "%25");
                result.Add(key, value);
            }
            return result;
        }

        private HttpRequestMessage PrepareRequest(string endpoint, HttpMethod httpMethod, Dictionary<string, string> parameters = null)
        {

            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            if (httpMethod == HttpMethod.Get)
            {
                //ignore limit
                if (!parameters.Keys.Contains("filter[limit]"))
                {
                    parameters["filter[limit]"] = "-1";
                }
             
            }

            //if (content != null)
            //    parameters["data"] = content;

            parameters["oauth_consumer_key"] = this.ConsumerKey;
            parameters["oauth_timestamp"] = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds.ToString();
               

            string seperator = parameters["oauth_timestamp"].Contains(",") ? "," : ".";

            parameters["oauth_timestamp"] = parameters["oauth_timestamp"].Substring(0,
                parameters["oauth_timestamp"].IndexOf(seperator));
            parameters["oauth_nonce"] = Hash(parameters["oauth_timestamp"]);
            parameters["oauth_signature_method"] = "HMAC-SHA256";
            parameters["oauth_signature"] = GenerateSignature(parameters, httpMethod.Method, endpoint);

            //parameters.Remove("data");
         

            StringBuilder sb = new StringBuilder();
            foreach (var pair in parameters)
            {
                sb.AppendFormat("&{0}={1}", HttpUtility.UrlEncode(pair.Key), HttpUtility.UrlEncode(pair.Value));
            }

            var path = "?" + sb.ToString().Substring(1).Replace("%5b", "%5B").Replace("%5d", "%5D");

            string baseAddress = this.StoreUrl + endpoint;
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(baseAddress);
          

            var request = new HttpRequestMessage(httpMethod, path);
            return request;
        }
        private string GenerateRequestUrl(string endpoint, HttpMethod httpMethod, Dictionary<string, string> parameters = null)
        {

            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            //if (httpMethod == HttpMethod.Get)
            //{
            //    //ignore limit
            //    if (!parameters.Keys.Contains("filter[limit]"))
            //    {
            //        
            //    }

            //}

            parameters["filter[meta]"] = "true";

        

            parameters["oauth_consumer_key"] = this.ConsumerKey;
            parameters["oauth_timestamp"] = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds.ToString();


            string seperator = parameters["oauth_timestamp"].Contains(",") ? "," : ".";

            parameters["oauth_timestamp"] = parameters["oauth_timestamp"].Substring(0,
            parameters["oauth_timestamp"].IndexOf(seperator));
            parameters["oauth_nonce"] = Hash(parameters["oauth_timestamp"]);
            parameters["oauth_signature_method"] = "HMAC-SHA256";
            parameters["oauth_signature"] = GenerateSignature(parameters, httpMethod.Method, endpoint);

            //parameters.Remove("data");


            StringBuilder sb = new StringBuilder();
            foreach (var pair in parameters)
            {
                sb.AppendFormat("&{0}={1}", HttpUtility.UrlEncode(pair.Key), HttpUtility.UrlEncode(pair.Value));
            }

            var path = "?" + sb.ToString().Substring(1).Replace("%5b", "%5B").Replace("%5d", "%5D");

            string baseAddress = this.StoreUrl + endpoint;

            return baseAddress+path;
        }

        private async Task<HttpResponseMessage> ExecuteRequest(HttpRequestMessage request)
        {
            HttpResponseMessage response;

            response = await httpClient.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
           
            return response;
        }

       private async Task<T> ProcessResponse<T>(HttpResponseMessage httpMessage)
        {
            Exception exception = null;
            string strResult=null;
            if (httpMessage.IsSuccessStatusCode || (httpMessage.StatusCode == System.Net.HttpStatusCode.NotModified))
            {
                try
                {
                     strResult = await httpMessage.Content.ReadAsStringAsync();

                    
                    JObject obj = JObject.Parse(strResult);
                    T data = default(T);
                    if (obj != null)
                    {
                        var root = obj.First;
                        if (root != null)
                        {
                            var rootJson = root.First;
                            if (rootJson != null)
                            {
                                data = rootJson.ToObject<T>();
                            }
                        }
                    }

                    return data;
                }
                catch (Exception e)
                {
                    exception = e;
                }
            }
            if (exception != null)
            {
                
            }
            return default(T);
        }


    
       internal async Task<string> Get(string apiEndpoint, Dictionary<string, string> parameters = null)
       {
           return await MakeApiDownloadStringCall(apiEndpoint, parameters);
       }
       internal async Task<string> Post(string apiEndpoint, Dictionary<string, string> parameters = null, string jsonData = null)
       {
           return await MakeApiUploadStringCall(HttpMethod.Post, apiEndpoint, parameters, jsonData);
       }
       internal async Task<string> Delete(string apiEndpoint, Dictionary<string, string> parameters = null, string jsonData = null)
       {
           return await MakeApiUploadStringCall(HttpMethod.Delete, apiEndpoint, parameters, jsonData);
       }
       internal async Task<string> Put(string apiEndpoint, Dictionary<string, string> parameters = null, string jsonData = null)
       {
           return await MakeApiUploadStringCall(HttpMethod.Put, apiEndpoint, parameters, jsonData);
       }


        //base
       private async Task<string> MakeApiDownloadStringCall(string apiEndpoint, Dictionary<string, string> parameters = null)
        {
            var url = GenerateRequestUrl(apiEndpoint, HttpMethod.Get, parameters);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return await client.GetStringAsync(url);
            }
        }
        
       private async Task<string> MakeApiUploadStringCall(HttpMethod httpMethod, string apiEndpoint, Dictionary<string, string> parameters = null, string jsonData = null)
       {
           var url = GenerateRequestUrl(apiEndpoint, httpMethod, parameters);
           using (var client = new HttpClient())
           {
               using (var content = new StringContent(jsonData ?? string.Empty))
               {
                   client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                   if (httpMethod == HttpMethod.Post)
                       return await (await client.PostAsync(url, content)).Content.ReadAsStringAsync();
                   else if (httpMethod == HttpMethod.Put)
                       return await (await client.PutAsync(url, content)).Content.ReadAsStringAsync();
                   else if (httpMethod == HttpMethod.Delete)
                       return await (await client.DeleteAsync(url)).Content.ReadAsStringAsync();
                   else throw new ArgumentException("only POST, PUT, and DELETE allowed for this HTTP Method");
               }
           }
       }
        
    }
}
