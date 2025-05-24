using Microsoft.AspNetCore.Mvc;
using RandoX.Data.Models.ProductModel;
using RandoX.Service.Interfaces;

namespace RandoX.API.Controllers
{
    public class ProductController : BaseAPIController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(int pageNumber, int pageSize)
        {
            var courses = await _productService.GetAllProductsAsync(pageNumber, pageSize);
            return Ok(courses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducts(ProductRequest productRequest)
        {
            var productResponse = await _productService.CreateProductAsync(productRequest);
            return Ok(productResponse);
        }
    }
}
