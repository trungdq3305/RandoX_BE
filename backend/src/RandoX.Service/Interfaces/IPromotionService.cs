using RandoX.Data.Entities;
using RandoX.Data.Models;
using RandoX.Data.Models.PromotionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Interfaces
{
    public interface IPromotionService
    {
        Task<ApiResponse<IEnumerable<Promotion>>> GetAllPromotionsAsync();
        Task<ApiResponse<Promotion>> GetPromotionByIdAsync(string id);
        Task<ApiResponse<Promotion>> CreatePromotionAsync(PromotionRequest promotion);
        Task<ApiResponse<Promotion>> UpdatePromotionAsync(string id, PromotionRequest promotion);
        Task<ApiResponse<Promotion>> DeletePromotionAsync(string id);
    }
}
