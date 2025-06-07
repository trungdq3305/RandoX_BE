using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Models
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public object? Data { get; set; }
        public string? Message { get; set; }
    }
}
