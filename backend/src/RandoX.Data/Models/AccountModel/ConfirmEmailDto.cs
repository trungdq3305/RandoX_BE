using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Models.AccountModel
{
    public class ConfirmEmailDto
    {
        public string? Token { get; set; }
        public string? Email { get; set; }
    }
}
