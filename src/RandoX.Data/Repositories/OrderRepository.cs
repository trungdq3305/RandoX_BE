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
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly randox_dbContext _context;
        private readonly IUnitOfWork _uow;
        public OrderRepository(randox_dbContext context, IUnitOfWork uow) : base(context)
        {
            _context = context;
            _uow = uow;
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await Entities.FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
