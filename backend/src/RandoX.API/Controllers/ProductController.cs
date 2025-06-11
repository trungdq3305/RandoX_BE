using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandoX.Data.Models.ProductModel;
using RandoX.Service.Interfaces;
using RandoX.Service.Services;
using System.Security.Claims;

namespace RandoX.API.Controllers
{
    [Authorize]
    public class ProductController : BaseAPIController
    {
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;
        public ProductController(IProductService productService, IAccountService accountService)
        {
            _productService = productService;
            _accountService = accountService;
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
        [HttpPut("productset-promotion")]
        public async Task<IActionResult> UpdateManuProToProduct([FromQuery] string id, [FromQuery] string? setId, [FromQuery] string? proId)
        {
            var productResponse = await _productService.UpdateSetProToProductAsync(id, setId, proId);
            return Ok(productResponse);
        }

        [HttpDelete("promotion")]
        public async Task<IActionResult> DeletePromotion(string id)
        {
            var productResponse = await _productService.DeletePromotionAsync(id);
            return Ok(productResponse);
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddProducttoCart(string productId)
        {
            var identity = this.HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null || !identity.IsAuthenticated)
                return Unauthorized("Bạn chưa đăng nhập");

            var claims = identity.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var user = await _accountService.GetAccountByEmailAsync(email);

            var productResponse = await _productService.AddProductToCartAsync(user.Id, productId);
            return Ok(productResponse);
        }
    }
}
