using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using WebApiEcomm.Core.Entites.Basket;
using WebApiEcomm.Core.Services;

namespace WebApiEcomm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        [HttpPost("Create/{basketId}")]
        public async Task<ActionResult<CustomerBasket>> Create(string basketId , int deliveryid )
        {
            return await paymentService.CreateOrUpdatePaymentAsync(basketId , deliveryid);
        }
    }
}
