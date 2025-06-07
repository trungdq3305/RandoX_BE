using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Models.AccountModel
{
    public class ResetPasswordConfirmDto
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
