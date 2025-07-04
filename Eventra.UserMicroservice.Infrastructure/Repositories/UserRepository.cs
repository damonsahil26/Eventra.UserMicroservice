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
    }
}
