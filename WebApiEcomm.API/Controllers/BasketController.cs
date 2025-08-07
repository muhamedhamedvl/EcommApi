using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEcomm.API.Helper;
using WebApiEcomm.Core.Entites.Basket;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;

namespace WebApiEcomm.API.Controllers
{

    public class BasketController : BaseController
    {
        public BasketController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("Get-Basket-Item/{id}")]
        public async Task<IActionResult> GetBasket(string id)
        {
            var res = await _work.CustomerBasketRepository.GetCustomerBasketAsync(id);
            if (res is null) 
            { 
                return Ok(new CustomerBasket());

            }
            return Ok(res);
        }
        [HttpPut("update-Basket")]
        public async Task<IActionResult> Update(CustomerBasket customerBasket)
        {
            var basket = await _work.CustomerBasketRepository.UpdateCustomerBasketAsync(customerBasket);
            return Ok(basket);
        }
        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            var res = await _work.CustomerBasketRepository.DeleteCustomerBasketAsync(id);
            return res ? Ok(new ResponseApi(200 , "Basket Deleted")) : BadRequest(new ResponseApi(400));
        }
    }
}
