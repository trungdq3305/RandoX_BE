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
    public class PromotionRepository : Repository<Promotion>, IPromotionRepository
    {
        private readonly randox_dbContext _context;
        private readonly IUnitOfWork _uow;

        public PromotionRepository(randox_dbContext context, IUnitOfWork uow) : base(context)
        {
            _context = context;
            _uow = uow;
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            return await Entities.Where(a => a.IsDeleted !=1).ToListAsync();
        }

        public async Task<Promotion> GetPromotionByIdAsync(string id)
        {
            return await Entities.Where(a => a.IsDeleted != 1).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Promotion> CreatePromotionAsync(Promotion promotion)
        {
            Entities.Add(promotion);
            await _uow.SaveChangesAsync();
            return promotion;
        }

        public async Task<Promotion> UpdatePromotionAsync(Promotion promotion)
        {
            Entities.Update(promotion);
            await _uow.SaveChangesAsync();
            return promotion;
        }
    }

}
