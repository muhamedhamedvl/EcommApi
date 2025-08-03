using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiEcomm.API.Helper;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;
using WebApiEcomm.Core.Services;
using WebApiEcomm.Core.Sharing;

namespace WebApiEcomm.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IImageManagementService imageManagementService;
        public ProductsController(IUnitOfWork work, IMapper mapper, IImageManagementService imageManagementService) : base(work, mapper)
        {
            this.imageManagementService = imageManagementService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductParams productParams)
        {
            try
            {
                var products = await 
                    _work
                    .ProductRepository
                    .GetAllAsync(productParams);
                return Ok(products);
                
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

                var res = mapper.Map<ProductDto>(product);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApi(400, $"Error: {ex.Message}"));
            }
        }

        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct(AddProductDto productdto)
        {
            try
            {
                var isAdded = await _work.ProductRepository.AddAsync(productdto);

                if (!isAdded)
                    return BadRequest(new ResponseApi(400, "Failed to add product."));

                return Ok(new ResponseApi(200, "Product added successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApi(400, $"Error: {ex.Message}"));
            }
        }

        [HttpPut("Update-Product")]
        public async Task<IActionResult> Update(UpdateProductDto updateProductDTO)
        {
            try
            {
                await _work.ProductRepository.UpdateAsync(updateProductDTO);
                return Ok(new ResponseApi(200));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete-Product/{Id}")]
        public async Task<IActionResult> delete(int Id)
        {
            try
            {
                var product = await _work.ProductRepository
                    .GetByIdAsync(Id, x => x.Photos, x => x.Category);

                await _work.ProductRepository.DeleteAsync(product);

                return Ok(new ResponseApi(200, "Product has been deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(error: ex.Message);
            }
        }
    }
}
