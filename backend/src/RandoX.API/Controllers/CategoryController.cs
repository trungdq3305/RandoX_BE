using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandoX.Data.Entities;
using RandoX.Data.Models.Category;
using RandoX.Service.Interfaces;

namespace RandoX.API.Controllers
{
    [Authorize]
    public class CategoryController : BaseAPIController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest categoryRequest)
        {
            var response = await _categoryService.CreateCategoryAsync(categoryRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] CategoryRequest categoryRequest)
        {
            var response = await _categoryService.UpdateCategoryAsync(id, categoryRequest);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);
            return Ok(response);
        }
    }

}
