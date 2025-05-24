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
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly randox_dbContext _context;
        private readonly IUnitOfWork _uow;

        public AccountRepository(randox_dbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Account?> GetAccount(string email, string password)
        {
            var user = await Entities.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return isValid ? user : null;
        }

        public async Task<Account> AddAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

    }
}
