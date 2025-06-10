using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Models.ProductModel
{
    public class ProductRequest
    {
        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public int? Quantity { get; set; }

        public decimal Price { get; set; }

        public string? ManufacturerId { get; set; }

        public string? CategoryId { get; set; }
    }
}
