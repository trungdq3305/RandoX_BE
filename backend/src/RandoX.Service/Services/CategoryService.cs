using RandoX.Data.Entities;
using RandoX.Data.Interfaces;
using RandoX.Data.Models;
using RandoX.Data.Models.Category;
using RandoX.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ApiResponse<IEnumerable<Category>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                return ApiResponse<IEnumerable<Category>>.Success(categories, "success");
            }
            catch (Exception)
            {
                return ApiResponse<IEnumerable<Category>>.Failure("Fail to get categories");
            }
        }

        public async Task<ApiResponse<Category>> GetCategoryByIdAsync(string id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                return ApiResponse<Category>.Success(category, "success");
            }
            catch (Exception)
            {
                return ApiResponse<Category>.Failure("Fail to get category");
            }
        }

        public async Task<ApiResponse<Category>> CreateCategoryAsync(CategoryRequest categoryRequest)
        {
            try
            {
                Category category = new Category
                {
                    Id = Guid.NewGuid().ToString(),
                    CategoryName = categoryRequest.CategoryName,
                    Description = categoryRequest.Description
                };

                await _categoryRepository.CreateCategoryAsync(category);

                return ApiResponse<Category>.Success(category, "Category created successfully");
            }
            catch (Exception)
            {
                return ApiResponse<Category>.Failure("Fail to create category");
            }
        }

        public async Task<ApiResponse<Category>> UpdateCategoryAsync(string id, CategoryRequest categoryRequest)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return ApiResponse<Category>.Failure("Category not found");
                }

                category.CategoryName = categoryRequest.CategoryName;
                category.Description = categoryRequest.Description;

                await _categoryRepository.UpdateCategoryAsync(category);

                return ApiResponse<Category>.Success(category, "Category updated successfully");
            }
            catch (Exception)
            {
                return ApiResponse<Category>.Failure("Fail to update category");
            }
        }
        public async Task<ApiResponse<Category>> DeleteCategoryAsync(string id)
        {
            try
            {
                var cat = await _categoryRepository.GetCategoryByIdAsync(id);
                cat.DeletedAt = DateTime.Now;
                cat.IsDeleted = 1;
                await _categoryRepository.UpdateCategoryAsync(cat);
                return ApiResponse<Category>.Success(cat, "Category delete successfully");
            }
            catch (Exception)
            {
                return ApiResponse<Category>.Failure("Fail to delete category");
            }
        }
    }

}
