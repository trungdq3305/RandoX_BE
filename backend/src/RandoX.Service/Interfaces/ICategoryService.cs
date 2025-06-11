using RandoX.Common;
using RandoX.Data.Entities;
using RandoX.Data.Models;
using RandoX.Data.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<PaginationResult<Category>>> GetAllCategoriesAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Category>> GetCategoryByIdAsync(string id);
        Task<ApiResponse<Category>> CreateCategoryAsync(CategoryRequest category);
        Task<ApiResponse<Category>> UpdateCategoryAsync(string id, CategoryRequest category);
        Task<ApiResponse<Category>> DeleteCategoryAsync(string id);
    }
}
