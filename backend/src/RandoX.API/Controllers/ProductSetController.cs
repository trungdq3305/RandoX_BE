using Microsoft.AspNetCore.Mvc;
using RandoX.Data.Models.ProductSetModel;
using RandoX.Service.Interfaces;
using RandoX.Service.Services;

namespace RandoX.API.Controllers
{
    public class ProductSetController : BaseAPIController
    {
        private readonly IProductSetService _productSetService;

        public ProductSetController(IProductSetService productSetService)
        {
            _productSetService = productSetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductSets(int pageNumber, int pageSize)
        {
            var response = await _productSetService.GetAllProductSetsAsync(pageNumber, pageSize);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductSetById(string id)
        {
            var response = await _productSetService.GetProductSetByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductSet([FromBody] ProductSetRequest productSetRequest)
        {
            var response = await _productSetService.CreateProductSetAsync(productSetRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductSet(string id, [FromBody] ProductSetRequest productSetRequest)
        {
            var response = await _productSetService.UpdateProductSetAsync(id, productSetRequest);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductSet(string id)
        {
            var response = await _productSetService.DeleteProductSetAsync(id);
            return Ok(response);
        }
        [HttpPut("promotion")]
        public async Task<IActionResult> UpdatePromotionToProductSet(string id, string proId)
        {
            var response = await _productSetService.UpdateProToProductSetAsync(id, proId);
            return Ok(response);
        }
        [HttpDelete("promotion")]
        public async Task<IActionResult> DeletePromotion(string id)
        {
            var productResponse = await _productSetService.DeletePromotionAsync(id);
            return Ok(productResponse);
        }
    }
}
