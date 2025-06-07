using RandoX.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Interfaces
{
    public interface IEmailTokenRepository
    {
        Task<EmailToken> CreateTokenAsync(EmailToken token);
        Task<EmailToken> GetTokenAsync(string token, string tokenType);
        Task<EmailToken> UpdateTokenAsync(EmailToken token);
        Task<bool> ValidateTokenAsync(string token, string email, string tokenType);
    }
}
