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
            try
            {
                var claims = new[]
                  {
                new Claim(ClaimTypes.Email, email),
                new Claim("UserId", userId.ToString())
            };

                var section = _config.GetSection("Jwt");

                var tokenOptions = new TokenOptionsDto
                {
                    Issuer = section.GetRequiredSection("Issuer").Value ?? string.Empty,
                    Audience = section.GetRequiredSection("Audience").Value ?? string.Empty,
                    ExpiryMinutes = Int32.Parse(section.GetRequiredSection("Expiry").Value ?? "10"),
                    SecretKey = section.GetRequiredSection("Key").Value ?? string.Empty
                };

                var token = _tokenService.GenerateToken(claims, tokenOptions);

                return token;
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }
    }
}
