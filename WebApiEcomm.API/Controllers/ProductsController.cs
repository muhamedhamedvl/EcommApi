using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEcomm.API.Helper;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;

namespace WebApiEcomm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await 
                    _work
                    .ProductRepository
                    .GetAllAsync(x => x.Category , x => x.Photos);

                var res = mapper.Map<List<ProductDto>>(products);
                if (res == null )
                {
                    return NotFound(new ResponseApi(404, "No products found."));
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _work.ProductRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }
    }
}
