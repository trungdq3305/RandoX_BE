using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RandoX.API.Helper;
using RandoX.Data.Entities;
using RandoX.Data.Interfaces;
using RandoX.Data.Models;
using RandoX.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly S3Service _s3Service;
        public ImageService(IImageRepository imageRepository, S3Service s3Service)
        {
            _imageRepository = imageRepository;
            _s3Service = s3Service;
        }
        public async Task<ResultModel> AddImageAsync(IFormFile image, string productId, string orderId)
        {
            ResultModel resultModel = new()
            {
                IsSuccess = false,
                Code = (int)HttpStatusCode.InternalServerError,
                Message = $"Internal Server Error"
            };
            if (image == null || image.Length == 0)
                return resultModel;

            var imageUrl = await _s3Service.UploadFileAsync(image);

            var imageEntity = new Image
            {
                Id = Guid.NewGuid().ToString(),
                ImageUrl = imageUrl,
                ProductId = productId,
                OrderId = orderId,
                CreatedAt = DateTime.UtcNow
            };

            await _imageRepository.AddImageAsync(imageEntity);

            resultModel.Data = new { imageUrl };
            return resultModel;
        }
    }
}
