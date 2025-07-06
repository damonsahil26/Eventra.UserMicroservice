using Eventra.UserMicroservice.Application.DTO;
using Eventra.UserMicroservice.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Eventra.UserMicroservice.Application.Services
{
    public class EmailTokenService : IEmailTokenService
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public EmailTokenService(IConfiguration config, ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }

        public string GenerateEmailConfirmationToken(Guid userId, string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("UserId", userId.ToString())
            };

            var tokenOptions = new TokenOptionsDto
            {
                Issuer = _config["Jwt.Issuer"] ?? string.Empty,
                Audience = _config["Jwt.Audience"] ?? string.Empty,
                ExpiryMinutes = Int32.Parse(_config["Jwt.Expiry"] ?? "10"),
                SecretKey = _config["Jwt.Key"] ?? string.Empty
            };

            var token = _tokenService.GenerateToken(claims, tokenOptions);

            return token;
        }
    }
}
