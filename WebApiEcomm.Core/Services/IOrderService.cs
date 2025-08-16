using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Order;

namespace WebApiEcomm.Core.Services
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(OrderDto orderDto , string BuyerEmail);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string BuyerEmail);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string BuyerEmail);
        Task<bool> CancelOrderAsync(int orderId);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
