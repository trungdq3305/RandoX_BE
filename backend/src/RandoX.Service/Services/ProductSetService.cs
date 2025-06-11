using RandoX.Common;
using RandoX.Data.Entities;
using RandoX.Data.Interfaces;
using RandoX.Data.Models;
using RandoX.Data.Models.ProductSetModel;
using RandoX.Data.Repositories;
using RandoX.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Services
{
    public class ProductSetService : IProductSetService
    {
        private readonly IProductSetRepository _productSetRepository;

        public ProductSetService(IProductSetRepository productSetRepository)
        {
            _productSetRepository = productSetRepository;
        }

        public async Task<ApiResponse<PaginationResult<ProductSet>>> GetAllProductSetsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var productSets = await _productSetRepository.GetAllProductSetsAsync();
                var paginatedResult = new PaginationResult<ProductSet>(productSets.ToList(), productSets.Count(), pageNumber, pageSize);
                return ApiResponse<PaginationResult<ProductSet>>.Success(paginatedResult, "Successfully retrieved product sets.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResult<ProductSet>>.Failure("Failed to retrieve product sets.");
            }
        }

        public async Task<ApiResponse<ProductSet>> GetProductSetByIdAsync(string id)
        {
            try
            {
                var productSet = await _productSetRepository.GetProductSetByIdAsync(id);
                return ApiResponse<ProductSet>.Success(productSet, "Successfully retrieved product set.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductSet>.Failure("Failed to retrieve product set.");
            }
        }

        public async Task<ApiResponse<ProductSet>> CreateProductSetAsync(ProductSetRequest productSetRequest)
        {
            try
            {
                var productSet = new ProductSet
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductSetName = productSetRequest.ProductSetName,
                    Description = productSetRequest.Description,
                    SetQuantity = productSetRequest.SetQuantity,
                    Quantity = productSetRequest.Quantity,
                    Price = productSetRequest.Price,
                };

                await _productSetRepository.CreateProductSetAsync(productSet);

                return ApiResponse<ProductSet>.Success(productSet, "Product set created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductSet>.Failure("Failed to create product set.");
            }
        }

        public async Task<ApiResponse<ProductSet>> UpdateProductSetAsync(string id, ProductSetRequest productSetRequest)
        {
            try
            {
                var productSet = await _productSetRepository.GetProductSetByIdAsync(id);
                if (productSet == null)
                {
                    return ApiResponse<ProductSet>.Failure("Product set not found.");
                }

                productSet.ProductSetName = productSetRequest.ProductSetName;
                productSet.Description = productSetRequest.Description;
                productSet.SetQuantity = productSetRequest.SetQuantity;
                productSet.Quantity = productSetRequest.Quantity;
                productSet.Price = productSetRequest.Price;

                await _productSetRepository.UpdateProductSetAsync(productSet);

                return ApiResponse<ProductSet>.Success(productSet, "Product set updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductSet>.Failure("Failed to update product set.");
            }
        }

        public async Task<ApiResponse<ProductSet>> DeleteProductSetAsync(string id)
        {
            try
            {
                var productSet = await _productSetRepository.DeleteProductSetAsync(id);
                return productSet != null ? ApiResponse<ProductSet>.Success(productSet, "Product set deleted successfully.") : ApiResponse<ProductSet>.Failure("Product set not found.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductSet>.Failure("Failed to delete product set.");
            }
        }
        public async Task<ApiResponse<ProductSet>> UpdateProToProductSetAsync(string id, string proId)
        {
            try
            {
                var productSet = await _productSetRepository.GetProductSetByIdAsync(id);
                if (productSet == null)
                {
                    return ApiResponse<ProductSet>.Failure("Product set not found.");
                }

                productSet.PromotionId = proId;

                await _productSetRepository.UpdateProductSetAsync(productSet);

                return ApiResponse<ProductSet>.Success(productSet, "promotion updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductSet>.Failure("Failed to update product set.");
            }
        }
        public async Task<ApiResponse<ProductSet>> DeletePromotionAsync(string id)
        {
            try
            {
                ProductSet productSet = await _productSetRepository.GetProductSetByIdAsync(id);

                productSet.PromotionId = null;


                await _productSetRepository.UpdateProductSetAsync(productSet);

                return ApiResponse<ProductSet>.Success(productSet, "success");
            }
            catch (Exception)
            {
                return ApiResponse<ProductSet>.Failure("Fail to delete promotion ");
            }
        }
    }

}
