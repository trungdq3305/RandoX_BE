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
    }
}
