using Microsoft.EntityFrameworkCore;
using RandoX.Data.Bases;
using RandoX.Data.DBContext;
using RandoX.Data.Entities;
using RandoX.Data.Interfaces;
using RandoX.Data.Models;
using RandoX.Data.Models.ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly randox_dbContext _context;
        private readonly IUnitOfWork _uow;
        public ProductRepository(randox_dbContext context, IUnitOfWork uow) : base(context)
        {
            _context = context;
            _uow = uow;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await Entities.Where(a => a.IsDeleted != 1)
                //.Include(p => p.Manufacturer)
                //.Include(p => p.ProductSet)
                //.Include(p => p.Promotion)
                .ToListAsync();
            return products;
        }
        public async Task<Product> GetProductByIdAsync(string id)
        {
            var product = await Entities
                .Include(p => p.Manufacturer)
                .Include(p => p.ProductSet)
                .Include(p => p.Promotion)
                .Include(p => p.Category)
                .Where(a => a.IsDeleted != 1)
                .FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            Entities.Add(product);
            await _uow.SaveChangesAsync();
            return product;
        }
        public async Task<Product> UpdateProductAsync(Product product)
        {
            Entities.Update(product);
            await _uow.SaveChangesAsync();
            return product;
        }

        public async Task<CartProduct> AddProductToCartAsync(CartProduct cartProduct)
        {
            _context.CartProducts.Add(cartProduct);
            await _uow.SaveChangesAsync();
            return cartProduct;
        }
        public async Task<CartProduct> UpdateCartProductAsync(CartProduct cartProduct)
        {
            _context.CartProducts.Update(cartProduct);
            await _uow.SaveChangesAsync();
            return cartProduct;
        }
    }
}
