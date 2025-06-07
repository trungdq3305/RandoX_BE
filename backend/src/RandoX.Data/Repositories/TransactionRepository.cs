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
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly randox_dbContext _context;
        private readonly IUnitOfWork _uow;
        public TransactionRepository(randox_dbContext context, IUnitOfWork uow) : base(context)
        {
            _context = context;
            _uow = uow;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            Entities.Add(transaction);
            await _uow.SaveChangesAsync();
            return transaction;
        }
    }
}
