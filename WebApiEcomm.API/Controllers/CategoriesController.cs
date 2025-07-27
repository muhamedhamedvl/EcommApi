using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Product;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;

namespace WebApiEcomm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _work.CategoryRepository.GetAllAsync();
                if (categories == null || !categories.Any())
                {
                    return NotFound("No categories found.");
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _work.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory(CategoryDto categorydto)
        {
            if (categorydto == null)
            {
                return BadRequest("Category data is null.");
            }
            try
            {
                var newCategory = mapper.Map<Category>(categorydto);
                await _work.CategoryRepository.AddAsync(newCategory);
                return Ok(new { Message = "Item Has been Added" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating category: {ex.Message}");
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updatecategorydto)
        { 
            try
            {
                var  category = mapper.Map<Category>(updatecategorydto);
                await _work.CategoryRepository.UpdateAsync(category);
                return Ok(new { Message = "Item Has been Updated" });
            }
            catch(Exception ex)
            {
                throw new Exception($"Error updating category: {ex.Message}");
            }
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _work.CategoryRepository.GetByIdAsync(id);
                await _work.CategoryRepository.DeleteAsync(category);
                return Ok(new { Message = "Item has been deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                
            }
        } 
    } 
}
