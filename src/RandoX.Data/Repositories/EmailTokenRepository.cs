using Microsoft.EntityFrameworkCore;
using RandoX.Data.DBContext;
using RandoX.Data.Entities;
using RandoX.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Repositories
{
    public class EmailTokenRepository : IEmailTokenRepository
    {
        private readonly randox_dbContext _context;

        public EmailTokenRepository(randox_dbContext context)
        {
            _context = context;
        }

        public async Task<EmailToken> CreateTokenAsync(EmailToken token)
        {
            token.Id = Guid.NewGuid().ToString();
            token.CreatedAt = DateTime.UtcNow;

            _context.EmailTokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<EmailToken> GetTokenAsync(string token, string tokenType)
        {
            return await _context.EmailTokens
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Token == token &&
                                   t.TokenType == tokenType &&
                                   t.IsUsed != 1 &&
                                   t.ExpiryDate > DateTime.UtcNow);
        }

        public async Task<EmailToken> UpdateTokenAsync(EmailToken token)
        {
            _context.EmailTokens.Update(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<bool> ValidateTokenAsync(string token, string email, string tokenType)
        {
            return await _context.EmailTokens
                .AnyAsync(t => t.Token == token &&
                         t.Account.Email == email &&
                         t.TokenType == tokenType &&
                         t.IsUsed != 1 &&
                         t.ExpiryDate > DateTime.UtcNow);
        }
    }
}
