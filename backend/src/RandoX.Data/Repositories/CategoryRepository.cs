using Microsoft.EntityFrameworkCore;
using RandoX.Data.Bases;
using RandoX.Data.DBContext;
using RandoX.Data.Entities;
using RandoX.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly randox_dbContext _context;
        private readonly IUnitOfWork _uow;

        public CategoryRepository(randox_dbContext context, IUnitOfWork uow) : base(context)
        {
            _context = context;
            _uow = uow;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await Entities.Where(a => a.IsDeleted != 1).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            return await Entities.Where(a => a.IsDeleted != 1).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            Entities.Add(category);
            await _uow.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            Entities.Update(category);
            await _uow.SaveChangesAsync();
            return category;
        }
    }

}
