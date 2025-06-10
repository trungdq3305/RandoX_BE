using RandoX.Data.Models.ProductModel;
using RandoX.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RandoX.Service.Interfaces
{
    public interface IImageService
    {
        Task<ApiResponse<string>> AddImageAsync(IFormFile image, string productId, string orderId);
    }
}
