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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> LoginUser(LoginUserRequest loginUser)
        {
            return await _userRepository.LoginUserAsync(loginUser.Email, loginUser.Password);
        }

        public async Task<bool> RegisterUser(RegisterUserRequest registerUser)
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

            return await _userRepository.RegisterUser(user, registerUser.Password);
        }
    }
}
