using RandoX.Common;
using RandoX.Data.Entities;
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
        Task<ApiResponse<PaginationResult<Product>>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Product>> GetProductByIdAsync(string id);
        Task<ApiResponse<ProductRequest>> CreateProductAsync(ProductRequest productRequest);
        Task<ApiResponse<ProductRequest>> UpdateProductAsync(string id, ProductRequest productRequest);
        Task<ApiResponse<Product>> DeleteProductAsync(string id);
    }
}
