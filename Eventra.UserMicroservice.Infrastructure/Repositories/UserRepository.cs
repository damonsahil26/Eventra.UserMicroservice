using Eventra.UserMicroservice.Domain.Models;
using Eventra.UserMicroservice.Infrastructure.Persistance;
using Eventra.UserMicroservice.Infrastructure.Repositories.Interfaces;
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

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AppUser> GetUser(Guid userId)
        {
            var user = await _dbContext.AppUsers
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return new();
            }

            return user;
        }

        public async Task<AppUser> GetUserWithEmail(string email)
        {
            var user = await _dbContext.AppUsers
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return new();
            }

            return user;
        }

        public async Task<bool> RegisterUser(AppUser user)
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

            await _dbContext.AppUsers.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task UpdateEmailConfirmation(Guid userId)
        {
            var user = await GetUser(userId);
            if (user == null)
            {
                return;
            }

            user.IsEmailConfirmed = true;
            user.ModifiedDate = DateTime.UtcNow;

            _dbContext.AppUsers.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
