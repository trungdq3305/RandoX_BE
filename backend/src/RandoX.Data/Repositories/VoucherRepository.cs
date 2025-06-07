
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
    public class VoucherRepository :  IVoucherRepository
    {
        private readonly randox_dbContext _context;
        public VoucherRepository(randox_dbContext context) 
        {
            _context = context;
        }

        public Task<IEnumerable<Voucher>> GetAllVouchersAsync()
        {
            throw new NotImplementedException();
        }
        //
    }
}
