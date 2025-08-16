using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Order;

namespace WebApiEcomm.Core.Entites.Dtos
{
    public record OrderDto
    {
        public int DelliveryMethodId { get; set; }
        public string basketId { get; set; }
        public ShippingAddressDto shipaddressdto { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
