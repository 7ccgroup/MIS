using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BaseAppServerCom.Helpers;
using BaseAppServerCom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom
{
    public partial class WooCommerceClient
    {
        #region Get
        public async Task<List<Product>> GetProducts()
        {

           

            var request = PrepareRequest("products", HttpMethod.Get);
            var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
            var result = await ProcessResponse<List<Product>>(response).ConfigureAwait(continueOnCapturedContext: false);


            return result;
        }

        public async Task<List<int>> GetIDs(Endpoint endpoint)
        {
                Dictionary<string, string> filter = new Dictionary<string, string>();
                filter.Add("fields", "id");

            var request = PrepareRequest(EndpointDefinition.GetValueByEndpoint(endpoint), HttpMethod.Get, filter);
            var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
            var result = await ProcessResponse<List<IDIdentifier>>(response).ConfigureAwait(continueOnCapturedContext: false);

            if (result == null) return new List<int>();

            var ids= await Task.Factory.StartNew<List<int>>(() => {

                return result.Select(n => n.id).ToList();
            }).ConfigureAwait(continueOnCapturedContext: false);

            return ids;

            
        }

        //public async Task<List<int>> GetProductIDs()
        //{
           

        //    var request = PrepareRequest("products", HttpMethod.Get,filter);
        //    var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
        //    var result = await ProcessResponse<List<int>>(response).ConfigureAwait(continueOnCapturedContext: false);


        //    return result;
        //}

        public async Task<Product> GetProductById(int id)
        {

            var request = PrepareRequest(string.Format("products/{0}", id), HttpMethod.Get);
            var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
            var result = await ProcessResponse<Product>(response).ConfigureAwait(continueOnCapturedContext: false);
            return result;

        }
        //public async Task<Customer> GetCustomerById(int id)
        //{

        //    var request = PrepareRequest(string.Format("customers/{0}", id), HttpMethod.Get);
        //    var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
        //    var result = await ProcessResponse<Customer>(response).ConfigureAwait(continueOnCapturedContext: false);
        //    return result;

        //}
        //public async Task<IEnumerable<Customer>> GetCustomerById(Dictionary<string, string> parameters = null)
        //{
        //    return (await Get<Customer_>(apiEndpoint: "customers", parameters: parameters)).Content;
        //}
       
        


        public async Task<List<Product>> GetProductsByCategorySlug(string catslug)
        {

            Dictionary<string, string> filter = new Dictionary<string,string>();
            filter.Add("filter[category]", catslug);


            var request = PrepareRequest("products", HttpMethod.Get,filter);
            var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
            var result = await ProcessResponse<List<Product>>(response).ConfigureAwait(continueOnCapturedContext: false);
            return result;

        }

        public async Task<List<Order>> GetOrders()
        {

            var request = PrepareRequest("orders", HttpMethod.Get);
            var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
            var result = await ProcessResponse<List<Order>>(response).ConfigureAwait(continueOnCapturedContext: false);
            return result;

        }

        public async Task<Order> GetOrderById(int id)
        {

            var request = PrepareRequest(string.Format("orders/{0}",id), HttpMethod.Get);
            var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
            var result = await ProcessResponse<Order>(response).ConfigureAwait(continueOnCapturedContext: false);
            return result;

        }

        public async Task<List<Category>> GetCategories()
        {
            var request = PrepareRequest("products/categories", HttpMethod.Get);
            var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
            var result = await ProcessResponse<List<Category>>(response).ConfigureAwait(continueOnCapturedContext: false);
            return result;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var request = PrepareRequest(string.Format("products/categories/{0}",id), HttpMethod.Get);
            var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
            var result = await ProcessResponse<Category>(response).ConfigureAwait(continueOnCapturedContext: false);
            return result;
        }

        public async Task<List<Tax>> GetTaxes()
        {

            var request = PrepareRequest("taxes", HttpMethod.Get);
            var response = await ExecuteRequest(request).ConfigureAwait(continueOnCapturedContext: false);
            var result = await ProcessResponse<List<Tax>>(response).ConfigureAwait(continueOnCapturedContext: false);
            return result;
        }
        #endregion
       

       
    }
}
