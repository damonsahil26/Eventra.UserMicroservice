using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventra.UserMicroservice.Application.Services.Interfaces
{
    public interface IEmailTokenService
    {
        public string GenerateEmailConfirmationToken(Guid userId, string email);
    }
}
