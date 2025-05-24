using RandoX.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Interfaces
{
    public interface IAccountService
    {
        Task<Account> Authenticate(string email, string password);
        Task<Account> Register(Account account);
    }
}
