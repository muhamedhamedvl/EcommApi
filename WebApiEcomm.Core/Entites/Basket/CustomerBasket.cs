using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEcomm.Core.Entites.Basket
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
            
        }
        public CustomerBasket(int Id)
        { 
            this.Id = Id;
        }
        public int Id { get; set; }
        public List<BasketItems> basketItems { get; set; } = new List<BasketItems>();
    }
}
