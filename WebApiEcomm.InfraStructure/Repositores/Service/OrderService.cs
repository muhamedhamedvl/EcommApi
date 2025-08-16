using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Order;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;
using WebApiEcomm.Core.Services;
using WebApiEcomm.InfraStructure.Data;

namespace WebApiEcomm.InfraStructure.Repositores.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, AppDbContext context, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.status == Status.Shipped || order.status == Status.Cancelled)
                return false;

            order.status = Status.Cancelled;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateOrderAsync(OrderDto orderDto, string BuyerEmail)
        {

            if (orderDto == null || string.IsNullOrWhiteSpace(BuyerEmail))
                return false;

            var deliveryMethod = await _context.DeliveryMethods.FindAsync(orderDto.DelliveryMethodId);
            if (deliveryMethod == null)
                return false;

            var shippingAddress = new ShippingAddressDto(
                 orderDto.shipaddressdto.FirstName,
                 orderDto.shipaddressdto.LastName,
                 orderDto.shipaddressdto.City,
                 orderDto.shipaddressdto.ZipCode,
                 orderDto.shipaddressdto.Street,
                 orderDto.shipaddressdto.State
             );

            var orderItems = new List<OrderItem>();
            decimal subTotal = 0;

            foreach (var item in orderDto.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    return false;

                var orderItem = new OrderItem(
                    product.Id,
                    product.Name,
                    product.NewPrice,
                    item.Quantity,
                    product.Photos?.FirstOrDefault()?.ImageName ?? string.Empty
                );
                orderItems.Add(orderItem);
                subTotal += product.NewPrice * item.Quantity;
            }

            var order = new Order(
                BuyerEmail,
                subTotal,
                shippingAddress,
                deliveryMethod,
                orderItems,
                paymentIntentId: null
            )
            {
                status = Status.Pending,
                OrderDate = DateTime.UtcNow
            };
            await _context.Orders.AddAsync(order);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found");
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _context.DeliveryMethods.ToListAsync();
            if (deliveryMethods == null)
                throw new KeyNotFoundException("Delivery methods not found");
            return deliveryMethods;
        }



        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string BuyerEmail)
        {
            if (string.IsNullOrWhiteSpace(BuyerEmail))
                throw new ArgumentException("Buyer email cannot be null or empty", nameof(BuyerEmail));
            var orders =await _context.Orders
                .Where(o => o.BuyerEmail == BuyerEmail)
                .Include(o => o.OrderItems)
                .Include(o => o.deliveryMethod)
                .Include(o => o.shippingaddress)
                .ToListAsync();
            return orders;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string BuyerEmail)
        {
            if (string.IsNullOrWhiteSpace(BuyerEmail))
                throw new ArgumentException("Buyer email cannot be null or empty", nameof(BuyerEmail));
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;
            if (order.status == Status.Shipped || order.status == Status.Cancelled)
                return false;
            order.status = Status.Shipped;
            order.BuyerEmail = BuyerEmail;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
