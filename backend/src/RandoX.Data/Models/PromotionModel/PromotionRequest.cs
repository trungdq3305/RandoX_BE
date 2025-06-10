using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Models.PromotionModel
{
    public class PromotionRequest
    {
        public string? Event { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? PercentageDiscountValue { get; set; }
        public decimal? DiscountValue { get; set; }
    }

}
