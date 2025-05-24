using RandoX.Data.Entities;
using RandoX.Data.Interfaces;
using RandoX.Data.Models;
using RandoX.Data.Models.ProductModel;
using RandoX.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ResultModel> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            ResultModel resultModel = new()
            {
                IsSuccess = false,
                Code = (int)HttpStatusCode.InternalServerError,
                Message = $"Internal Server Error"
            };
            try
            {
                var products = await _productRepository.GetAllProductsAsync(pageNumber, pageSize);
                

                return new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Data = products,
                    Message = "Get all products successfully"
                };
            }
            catch (Exception ex)
            {
                return resultModel;
            }
        }

        public async Task<ResultModel> CreateProductAsync(ProductRequest productRequest)
        {
            ResultModel resultModel = new()
            {
                IsSuccess = false,
                Code = (int)HttpStatusCode.InternalServerError,
                Message = $"Internal Server Error"
            };
            try
            {
                Product product = new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductName = productRequest.ProductName,
                    Description = productRequest.Description,
                    Quantity = productRequest.Quantity,
                    Price = productRequest.Price,
                    ManufacturerId = productRequest.ManufacturerId,
                    ProductSetId = productRequest.ProductSetId,
                    PromotionId = productRequest.PromotionId,
                };
                
                await _productRepository.CreateProductAsync(product);

                ProductResponse productResponse = new ProductResponse
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    ManufacturerId = product.ManufacturerId,
                    ProductSetId = product.ProductSetId,
                    PromotionId = productRequest.PromotionId,
                };
                return new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Data = productResponse,
                    Message = "Create new product successfully"
                };
            }
            catch (Exception ex)
            {
                resultModel.Message = ex.InnerException?.Message ?? ex.Message;
                return resultModel;
            }
        }

    }
}
