using RandoX.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Interfaces
{
    public interface IProductSetRepository
    {
        Task<ProductSet> CreateProductSetAsync(ProductSet productSet);
        Task<IEnumerable<ProductSet>> GetAllProductSetsAsync();
        Task<ProductSet> GetProductSetByIdAsync(string id);
        Task<ProductSet> UpdateProductSetAsync(ProductSet productSet);
        Task<ProductSet> DeleteProductSetAsync(string id);
    }

}
