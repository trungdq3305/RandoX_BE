using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RandoX.Common;
using RandoX.Data.DBContext;
using RandoX.Data.Entities;
using RandoX.Service.Interfaces;

namespace RandoX.API.Controllers
{
    [Authorize]
    public class ImageController : BaseAPIController
    {
        private readonly IImageService _imageService;
        
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile image, [FromQuery]string? productId, [FromQuery] string? orderId)
        {
            var res = await _imageService.AddImageAsync(image, productId, orderId);

            return Ok(res);
        }
    }
}
