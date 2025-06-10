using RandoX.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateProductAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<Product> UpdateProductAsync(Product product);

    }
}
