using RandoX.Data.Entities;
using RandoX.Data.Interfaces;
using RandoX.Data.Models.ProductModel;
using RandoX.Data.Models;
using RandoX.Data.Repositories;
using RandoX.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transctionRepository;

        public TransactionService(ITransactionRepository transctionRepository)
        {
            _transctionRepository = transctionRepository;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            await _transctionRepository.CreateTransactionAsync(transaction);
            return transaction;
        }
    }
}
