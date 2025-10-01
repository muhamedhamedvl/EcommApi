using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using WebApiEcomm.Core.Entites.Basket;
using WebApiEcomm.Core.Services;

namespace WebApiEcomm.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class paymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        public paymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        [HttpPost("baskets/{basketId}")]
        public async Task<ActionResult<CustomerBasket>> Create(string basketId , int deliveryid )
        {
            return await paymentService.CreateOrUpdatePaymentAsync(basketId , deliveryid);
        }
    }
}
