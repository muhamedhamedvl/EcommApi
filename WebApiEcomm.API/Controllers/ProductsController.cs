using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEcomm.API.Helper;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;
using WebApiEcomm.Core.Entites.Product; // if needed for entity access
using System.Linq.Expressions;

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
                var products = await _work.ProductRepository.GetAllAsync(
                    p => p.Category,
                    p => p.Photos
                );

                var res = mapper.Map<List<ProductDto>>(products);

                if (res == null || !res.Any())
                {
                    return NotFound(new ResponseApi(404, "No products found."));
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApi(400, $"Error: {ex.Message}"));
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _work.ProductRepository.GetByIdAsync(
                    id,
                    p => p.Category,
                    p => p.Photos
                );

                if (product == null)
                {
                    return NotFound(new ResponseApi(404, "Product not found."));
                }

                var productDto = mapper.Map<ProductDto>(product);

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
