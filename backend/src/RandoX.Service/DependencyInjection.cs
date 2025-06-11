using Microsoft.Extensions.DependencyInjection;
using RandoX.Data.Interfaces;
using RandoX.Service.Interfaces;
using RandoX.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection service)
        {
            service.AddTransient<IProductService, ProductService>();
            service.AddTransient<IAccountService, AccountService>();
            service.AddTransient<IImageService, ImageService>();
            service.AddTransient<IOrderService, OrderService>();
            service.AddTransient<ITransactionService, TransactionService>();
            service.AddTransient<IEmailService, EmailService>();
            service.AddTransient<IManufacturerService, ManufacturerService>();
            service.AddTransient<ICategoryService, CategoryService>();
            service.AddTransient<IProductService, ProductService>();
            service.AddTransient<IProductSetService, ProductSetService>();
            return service;
        }
    }
}
