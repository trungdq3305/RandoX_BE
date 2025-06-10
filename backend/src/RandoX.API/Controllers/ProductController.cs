using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandoX.Data.Models.ProductModel;
using RandoX.Service.Interfaces;

namespace RandoX.API.Controllers
{
    [Authorize]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var courses = await _productService.GetProductByIdAsync(id);
            return Ok(courses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductRequest productRequest)
        {
            var productResponse = await _productService.CreateProductAsync(productRequest);
            return Ok(productResponse);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string id, ProductRequest productRequest)
        {
            var productResponse = await _productService.UpdateProductAsync(id, productRequest);
            return Ok(productResponse);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var productResponse = await _productService.DeleteProductAsync(id);
            return Ok(productResponse);
        }
    }
}
