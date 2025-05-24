using Microsoft.Extensions.DependencyInjection;
using RandoX.Data.Bases;
using RandoX.Data.Interfaces;
using RandoX.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection service)
        {
            service.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            service.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            service.AddTransient<IProductRepository, ProductRepository>();
            service.AddTransient<IAccountRepository, AccountRepository>();
            return service;
        }
    }
}
