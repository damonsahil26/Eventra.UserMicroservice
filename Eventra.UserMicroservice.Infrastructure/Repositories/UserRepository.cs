using Eventra.UserMicroservice.Domain.Models;
using Eventra.UserMicroservice.Infrastructure.Persistance;
using Eventra.UserMicroservice.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventra.UserMicroservice.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PasswordHasher _passwordHasher = new();

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> LoginUserAsync(string email, string password)
        {
            var user = await _dbContext.AppUsers.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return false;
            }

            if (password == null)
            {
                return false;
            }

            var passwordVerificationResult = _passwordHasher
                .VerifyHashedPassword(user.PasswordHash, password);

            if(passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RegisterUser(AppUser user, string password)
        {
            if (user == null)
            {
                return false;
            }

            var userFromDb = await _dbContext.AppUsers
                .FirstOrDefaultAsync(x => x.UserName.Equals(user.UserName)
                || x.Email.Equals(user.Email));

            if(userFromDb != null)
            {
                return false;
            }

            user.PasswordHash = _passwordHasher.HashPassword(password);

            await _dbContext.AppUsers.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
