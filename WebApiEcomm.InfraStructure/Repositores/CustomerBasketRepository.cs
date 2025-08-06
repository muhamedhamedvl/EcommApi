using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Basket;
using WebApiEcomm.Core.Interfaces;

namespace WebApiEcomm.InfraStructure.Repositores
{
    internal class CustomerBasketRepository : ICustomerBasketRepository
    {
        public Task<bool> DeleteCustomerBasketAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerBasket> GetCustomerBasketAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerBasket> UpdateCustomerBasketAsync(CustomerBasket customerBasket)
        {
            throw new NotImplementedException();
        }
    }
}
