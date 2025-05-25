using RandoX.Data.Models.ProductModel;
using RandoX.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandoX.Data.Entities;

namespace RandoX.Service.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
    }
}
