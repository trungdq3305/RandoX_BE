using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandoX.Data.Entities;
using RandoX.Data.Models.PromotionModel;
using RandoX.Service.Interfaces;

namespace RandoX.API.Controllers
{
    [Authorize]
    public class PromotionController : BaseAPIController
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPromotions(int pageNumber, int pageSize)
        {
            var response = await _promotionService.GetAllPromotionsAsync(pageNumber, pageSize);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionById(string id)
        {
            var response = await _promotionService.GetPromotionByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] PromotionRequest promotionRequest)
        {
            var response = await _promotionService.CreatePromotionAsync(promotionRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePromotion(string id, [FromBody] PromotionRequest promotionRequest)
        {
            var response = await _promotionService.UpdatePromotionAsync(id, promotionRequest);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePromotion(string id)
        {
            var response = await _promotionService.DeletePromotionAsync(id);
            return Ok(response);
        }
    }

}
