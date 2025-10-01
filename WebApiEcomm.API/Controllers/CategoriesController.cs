using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEcomm.API.Helper;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Product;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;

namespace WebApiEcomm.API.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _work.CategoryRepository.GetAllAsync();
                if (categories == null || !categories.Any())
                {
                    return BadRequest(new ResponseApi(400));
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _work.CategoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto categorydto)
        {
            if (categorydto == null)
            {
                return BadRequest(new ResponseApi(400));
            }
            try
            {
                var newCategory = mapper.Map<Category>(categorydto);
                await _work.CategoryRepository.AddAsync(newCategory);
                return StatusCode(201 , new ResponseApi(201 , "category created"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, UpdateCategoryDto updatecategorydto)
        { 
            try
            {
                var  category = mapper.Map<Category>(updatecategorydto);
                category.Id = categoryId;
                await _work.CategoryRepository.UpdateAsync(category);
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest (ex.Message);
            }
        }
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            if(categoryId <= 0)
            {
                return BadRequest(new ResponseApi(400));
            }
            try
            {
                var category = await _work.CategoryRepository.GetByIdAsync(categoryId);
                await _work.CategoryRepository.DeleteAsync(category);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                
            }
        } 
    } 
}
