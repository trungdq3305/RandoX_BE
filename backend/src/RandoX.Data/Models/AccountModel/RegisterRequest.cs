using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Models.AccountModel
{
    public class RegisterRequest
    {
        public string? Email { get; set; }

        public DateOnly Dob { get; set; }

        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? RoleId { get; set; }
    }
}
