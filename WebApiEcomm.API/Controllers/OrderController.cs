﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Order;
using WebApiEcomm.Core.Services;

namespace WebApiEcomm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create-order")]
        public async Task<ActionResult> create(OrderDto orderDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            Order order = await _orderService.CreateOrdersAsync(orderDTO, email);

            return Ok(order);
        }


        [HttpGet("get-orders-for-user")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> getorders()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.GetAllOrdersForUserAsync(email);
            return Ok(order);
        }


        [HttpGet("get-order-by-id/{id}")]
        public async Task<ActionResult<OrderToReturnDTO>> getOrderById(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.GetOrderByIdAsync(id, email);
            return Ok(order);
        }


        [HttpGet("get-delivery")]
        public async Task<ActionResult> GetDeliver()
        => Ok(await _orderService.GetDeliveryMethodAsync());
    }
}
