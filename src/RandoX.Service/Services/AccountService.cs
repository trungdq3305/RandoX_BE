using RandoX.Data.Entities;
using RandoX.Data.Interfaces;
using RandoX.Data.Repositories;
using RandoX.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<Account> Authenticate(string email, string password)
        {
            return await _repository.GetAccount(email, password);
        }

        public async Task<Account> Register(Account account)
        {
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password); // hash password
            account.CreatedAt = DateTime.UtcNow;
            return await _repository.AddAsync(account);
        }
    }
}
