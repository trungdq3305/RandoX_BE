using RandoX.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccount(string email, string password);
        Task<Account> AddAsync(Account account);
        Task<Account> GetByEmailAsync(string email);
        Task<Account> GetByIdAsync(string id);
        Task<Account> CreateAsync(Account account);
        Task<Account> UpdateAsync(Account account);
        Task<bool> EmailExistsAsync(string email);
    }
}
