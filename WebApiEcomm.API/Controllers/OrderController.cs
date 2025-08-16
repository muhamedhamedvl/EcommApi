using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Services;

namespace WebApiEcomm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        private string GetUserEmail() =>
            User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty;

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderDto orderDto)
        {
            var email = GetUserEmail();
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("Invalid user email.");

            if (orderDto == null)
                return BadRequest("Invalid order data.");

            var result = await _orderService.CreateOrderAsync(orderDto, email);
            return result
                ? Ok("Order created successfully.")
                : BadRequest("Failed to create order.");
        }

        [HttpGet("GetOrdersByUserId")]
        public async Task<IActionResult> GetOrdersByUserIdAsync()
        {
            var email = GetUserEmail();
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("Invalid user email.");

            var orders = await _orderService.GetOrdersByUserIdAsync(email);
            return (orders == null || !orders.Any())
                ? NotFound("No orders found for this user.")
                : Ok(orders);
        }

        [HttpGet("GetOrderById/{orderId}")]
        public async Task<IActionResult> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            return order == null
                ? NotFound("Order not found.")
                : Ok(order);
        }

        [HttpGet("GetDeliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();
            return (deliveryMethods == null || !deliveryMethods.Any())
                ? NotFound("No delivery methods found.")
                : Ok(deliveryMethods);
        }

        [HttpPut("UpdateOrderStatus/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId)
        {
            var email = GetUserEmail();
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("Invalid user email.");

            var result = await _orderService.UpdateOrderStatusAsync(orderId, email);
            return result
                ? Ok("Order status updated successfully.")
                : NotFound("Order not found or already processed.");
        }
        [HttpDelete("CancelOrder/{orderId}")]
        public async Task<IActionResult> CancelOrderAsync(int orderId)
        {
            var email = GetUserEmail();
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("Invalid user email.");

            var result = await _orderService.CancelOrderAsync(orderId);
            return result
                ? Ok("Order cancelled successfully.")
                : NotFound("Order not found or already processed.");
        }
    }
}
