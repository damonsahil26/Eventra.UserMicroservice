using Eventra.Shared.DTO.Events;
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
        private readonly IMessagePublisher _messagePublisher;
        private readonly IEmailTokenService _emailTokenService;
        private readonly PasswordHasher<AppUser> _passwordHasher = new();

        public UserService(IUserRepository userRepository,
            IMessagePublisher messagePublisher,
            IEmailTokenService emailTokenService)
        {
            _userRepository = userRepository;
            _messagePublisher = messagePublisher;
            _emailTokenService = emailTokenService;
        }

        public async Task<bool> RegisterUser(RegisterUser registerUser)
        {
            try
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

                var token = _emailTokenService.GenerateEmailConfirmationToken(user.Id, user.Email);

                var message = new UserRegisteredEvent
                {
                    UserId = user.Id,
                    ConfirmationLink = $"https://localhost:8080/confirm-email?token={token}&email={user.Email}",
                    Email = user.Email,
                    FullName = user.FirstName + " " + user.LastName,
                };

                await _messagePublisher.PublishUserRegisteredAsync(message);

                return await _userRepository.RegisterUser(user);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
