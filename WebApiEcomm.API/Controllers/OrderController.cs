using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Order;
using WebApiEcomm.Core.Services;

namespace WebApiEcomm.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ordersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public ordersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<ActionResult> create(OrderDto orderDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            Order order = await _orderService.CreateOrdersAsync(orderDTO, email);

            return Ok(order);
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> getorders()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.GetAllOrdersForUserAsync(email);
            return Ok(order);
        }


        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderToReturnDTO>> getOrderById(int orderId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.GetOrderByIdAsync(orderId, email);
            return Ok(order);
        }


        [HttpGet("delivery-methods")]
        public async Task<ActionResult> GetDeliver()
        => Ok(await _orderService.GetDeliveryMethodAsync());
    }
}
