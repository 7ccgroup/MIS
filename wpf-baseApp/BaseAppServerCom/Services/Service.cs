using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom.Services
{
    public abstract class Service
    {
        protected readonly WooCommerceClient _client;

        protected Service(WooCommerceClient client)
        {
            _client = client;
        }

        protected async Task<T> Post<T>(string apiEndpoint, Dictionary<string, string> parameters = null, T toSerialize = default(T))
        {
            var jsonData = JsonConvert.SerializeObject(toSerialize);
            var jsonResult = await _client.Post(apiEndpoint, parameters, jsonData);
            return JsonConvert.DeserializeObject<T>(jsonResult);
        }

        protected async Task<T> Put<T>(string apiEndpoint, Dictionary<string, string> parameters = null, T toSerialize = default(T))
        {
            var jsonData = JsonConvert.SerializeObject(toSerialize);
            var jsonResult = await _client.Put(apiEndpoint, parameters, jsonData);
            return JsonConvert.DeserializeObject<T>(jsonResult);
        }

        protected async Task<T> Delete<T>(string apiEndpoint, Dictionary<string, string> parameters = null)
        {
            var jsonResult = await _client.Delete(apiEndpoint, parameters);
            return JsonConvert.DeserializeObject<T>(jsonResult);
        }

        protected async Task<T> Get<T>(string apiEndpoint, Dictionary<string, string> parameters = null)
        {
            var jsonResult = await _client.Get(apiEndpoint, parameters);
            return JsonConvert.DeserializeObject<T>(jsonResult);
        }
    }
}
