using Eventra.UserMicroservice.Application.DTO;
using Eventra.UserMicroservice.Application.Services.Interfaces;
using Eventra.UserMicroservice.Domain.Models;
using Eventra.UserMicroservice.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Eventra.UserMicroservice.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<AppUser> _passwordHasher = new();

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUser(RegisterUser registerUser)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                PhoneNumber = registerUser.PhoneNumber,
                UserTypes = registerUser.UserTypes
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, registerUser.Password);

            return await _userRepository.RegisterUser(user);
        }
    }
}
