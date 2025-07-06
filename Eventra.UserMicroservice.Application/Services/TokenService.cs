using Eventra.UserMicroservice.Application.DTO;
using Eventra.UserMicroservice.Application.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eventra.UserMicroservice.Application.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(IEnumerable<Claim> claims, TokenOptionsDto tokenOptions)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddMinutes(tokenOptions.ExpiryMinutes)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token, TokenOptionsDto tokenOptions)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(tokenOptions.SecretKey);

            try
            {
                var claimsPrincipal = jwtTokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = tokenOptions.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return claimsPrincipal ?? null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
