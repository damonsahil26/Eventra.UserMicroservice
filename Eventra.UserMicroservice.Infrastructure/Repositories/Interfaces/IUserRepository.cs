using Eventra.UserMicroservice.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventra.UserMicroservice.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> RegisterUser(AppUser user, string password);

        public Task<bool> LoginUserAsync(string email, string password);
    }
}
