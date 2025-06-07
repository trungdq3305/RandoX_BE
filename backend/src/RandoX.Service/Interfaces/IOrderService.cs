using RandoX.Data.Entities;
using RandoX.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(string orderId);
    }
}
