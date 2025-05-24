using RandoX.Data.Models;
using RandoX.Data.Models.ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Interfaces
{
    public interface IProductService
    {
        Task<ResultModel> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<ResultModel> CreateProductAsync(ProductRequest productRequest);
    }
}
