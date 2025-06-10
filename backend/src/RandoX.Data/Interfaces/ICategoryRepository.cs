using RandoX.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(string id);
        Task<Category> UpdateCategoryAsync(Category category);
    }

}
