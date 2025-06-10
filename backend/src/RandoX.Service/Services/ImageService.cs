using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RandoX.Common;
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
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly S3Service _s3Service;
        public ImageService(IImageRepository imageRepository, S3Service s3Service)
        {
            _imageRepository = imageRepository;
            _s3Service = s3Service;
        }
        public async Task<ApiResponse<string>> AddImageAsync(IFormFile image, string productId, string orderId)
        {
            if (image == null || image.Length == 0)
                return ApiResponse<string>.Failure("Internal Server Error");

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

            return ApiResponse<string>.Success(imageUrl, "success");
        }
    }
}
