using Eventra.UserMicroservice.Application.DTO;
using Eventra.UserMicroservice.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventra.UserMicroservice.Application.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _config;

        public TokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public TokenOptionsDto GetTokenOptions()
        {
            var section = _config.GetSection("Jwt");

            var tokenOptions = new TokenOptionsDto
            {
                Issuer = section.GetRequiredSection("Issuer").Value ?? string.Empty,
                Audience = section.GetRequiredSection("Audience").Value ?? string.Empty,
                ExpiryMinutes = Int32.Parse(section.GetRequiredSection("Expiry").Value ?? "10"),
                SecretKey = section.GetRequiredSection("Key").Value ?? string.Empty
            };

            return tokenOptions;
        }
    }
}
