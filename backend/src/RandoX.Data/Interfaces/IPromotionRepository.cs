using RandoX.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Interfaces
{
    public interface IPromotionRepository
    {
        Task<Promotion> CreatePromotionAsync(Promotion promotion);
        Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
        Task<Promotion> GetPromotionByIdAsync(string id);
        Task<Promotion> UpdatePromotionAsync(Promotion promotion);
    }

}
