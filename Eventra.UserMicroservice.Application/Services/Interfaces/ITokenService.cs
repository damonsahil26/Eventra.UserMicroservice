using Eventra.UserMicroservice.Application.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eventra.UserMicroservice.Application.Services.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(IEnumerable<Claim> claims, TokenOptionsDto tokenOptions);

        public ClaimsPrincipal? ValidateToken(string token, TokenOptionsDto tokenOptions);
    }
}
