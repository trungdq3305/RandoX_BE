using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Models.ProductSetModel
{
    public class ProductSetRequest
    {
        public string ProductSetName { get; set; }
        public string Description { get; set; }
        public int? SetQuantity { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }

}
