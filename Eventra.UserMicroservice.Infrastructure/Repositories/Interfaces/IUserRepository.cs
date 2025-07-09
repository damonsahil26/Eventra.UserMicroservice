using Eventra.UserMicroservice.Domain.Models;

namespace Eventra.UserMicroservice.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> RegisterUser(AppUser user);

        public Task<AppUser> GetUser(Guid userId);

        public Task<AppUser> GetUserWithEmail(string email);

        public Task UpdateEmailConfirmation(Guid userId);
    }
}
