using BaseAppServerCom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAppServerCom.Services
{
   public class CustomerService:Service
    {
       
       public CustomerService(WooCommerceClient client):base(client) {}
      

       public async Task<Customer> GetById(int customerId)
       {
           var endPoint = string.Format("customers/{0}", customerId);
           return (await Get<CustomerBundle>(endPoint)).Content;
       }
       public async Task<IEnumerable<Customer>> All(Dictionary<string, string> parameters = null)
       {
           return (await Get<CustomersBundle>(apiEndpoint: "customers", parameters: parameters)).Content;
       }

       public async Task<Customer> Create(Customer data)
       {
           var bundle = new CustomerBundle { Content = data };
           return (await Post(apiEndpoint: "customers", toSerialize: bundle)).Content;
       }

       public async Task<Customer> Update(int customerId, Customer newData)
       {
           var endPoint = String.Format("customers/{0}", customerId);
           var bundle = new CustomerBundle { Content = newData };
           return (await Put(endPoint, toSerialize: bundle)).Content;
       }

    }
}
