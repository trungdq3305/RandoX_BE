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
    public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
    {
        private readonly randox_dbContext _context;
        private readonly IUnitOfWork _uow;

        public ManufacturerRepository(randox_dbContext context, IUnitOfWork uow) : base(context)
        {
            _context = context;
            _uow = uow;
        }

        public async Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync()
        {
            return await Entities.Where(a => a.IsDeleted != 1).ToListAsync();
        }

        public async Task<Manufacturer> GetManufacturerByIdAsync(string id)
        {
            return await Entities.Where(a => a.IsDeleted != 1).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Manufacturer> CreateManufacturerAsync(Manufacturer manufacturer)
        {
            Entities.Add(manufacturer);
            await _uow.SaveChangesAsync();
            return manufacturer;
        }

        public async Task<Manufacturer> UpdateManufacturerAsync(Manufacturer manufacturer)
        {
            Entities.Update(manufacturer);
            await _uow.SaveChangesAsync();
            return manufacturer;
        }
    }

}
