using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RandoX.Data.Entities;
using RandoX.Data.Models.AccountModel;
using RandoX.Service.Interfaces;
using RandoX.Service.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RandoX.API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly IConfiguration _config;
        private readonly IAccountService _accountService;

        public AccountController(IConfiguration config, IAccountService accountService)
        {
            _config = config;
            _accountService = accountService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _accountService.Authenticate(request.Email, request.Password);

            if (user == null || user.Result == null)
                return Unauthorized();

            var token = GenerateJSONWebToken(user.Result);

            return Ok(token);
        }

        private string GenerateJSONWebToken(Account systemUserAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"]
                    , _config["Jwt:Audience"]
                    , new Claim[]
                    {
                new(ClaimTypes.Name, systemUserAccount.Email),
                //new(ClaimTypes.Email, systemUserAccount.Email),
                new(ClaimTypes.Role, systemUserAccount.RoleId.ToString()),
                    },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public sealed record LoginRequest(string Email, string Password);

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _accountService.Authenticate(request.Email, request.Password);
            if (existingUser != null)
                return BadRequest("Email already in use.");

            var newAccount = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                Password = request.Password, // sẽ được hash trong service
                PhoneNumber = request.PhoneNumber,
                Dob = request.Dob,
                RoleId = request.RoleId,
                Status = 1,
                IsDeleted = 0,
            };

            var result = await _accountService.Register(newAccount);
            return Ok(new
            {
                result.Id,
                result.Email,
                result.PhoneNumber,
                result.RoleId
            });
        }
    }
}
