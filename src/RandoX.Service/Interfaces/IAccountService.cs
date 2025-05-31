using RandoX.Data.Entities;
using RandoX.Data.Models.AccountModel;
using RandoX.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Interfaces
{
    public interface IAccountService
    {
        Task<Account> Authenticate(string email, string password);
        Task<Account> Register(Account account);
        Task<ApiResponse<Account>> RegisterAsync(RegisterRequest registerDto);
        Task<ApiResponse<bool>> ConfirmEmailAsync(ConfirmEmailDto confirmDto);
        Task<ApiResponse<bool>> RequestPasswordResetAsync(ResetPasswordDto resetDto);
        Task<ApiResponse<bool>> RequestPasswordChangeAsync(ChangePasswordDto changeDto);
        Task<ApiResponse<bool>> ConfirmPasswordResetAsync(string token, string newPassword);
        Task<ApiResponse<bool>> ConfirmPasswordChangeAsync(string token);
    }
}
