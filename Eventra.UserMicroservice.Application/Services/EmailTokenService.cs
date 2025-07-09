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
        private readonly ITokenGenerator _tokenGenerator;

        public EmailTokenService(IConfiguration config,
            ITokenService tokenService,
            ITokenGenerator tokenGenerator)
        {
            _config = config;
            _tokenService = tokenService;
            _tokenGenerator = tokenGenerator;
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

                var tokenOptions = _tokenGenerator.GetTokenOptions();

                var token = _tokenService.GenerateToken(claims, tokenOptions);

                return token;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
