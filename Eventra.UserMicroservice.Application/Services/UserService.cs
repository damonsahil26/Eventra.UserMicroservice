using Eventra.Shared.DTO.Events;
using Eventra.UserMicroservice.Application.DTO;
using Eventra.UserMicroservice.Application.Services.Interfaces;
using Eventra.UserMicroservice.Domain.Models;
using Eventra.UserMicroservice.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Eventra.UserMicroservice.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IEmailTokenService _emailTokenService;
        private readonly ITokenService _tokenService;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly PasswordHasher<AppUser> _passwordHasher = new();

        public UserService(IUserRepository userRepository,
            IMessagePublisher messagePublisher,
            IEmailTokenService emailTokenService,
            ITokenService tokenService,
            ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _messagePublisher = messagePublisher;
            _emailTokenService = emailTokenService;
            _tokenService = tokenService;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<RegisterUserRequest> GetRegisterdUser(Guid userId)
        {
            var appUser = await _userRepository.GetUser(userId);

            var registeredUser = new RegisterUserRequest
            {
                Id = userId,
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                PhoneNumber = appUser.PhoneNumber,
                UserName = appUser.UserName,
                UserTypes = appUser.UserTypes
            };

            return registeredUser;
        }

        public async Task<bool> RegisterUser(RegisterUserRequest registerUser)
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
                    ConfirmationLink = $"https://localhost:7047/api/Auth/confirm-email?token={token}&email={user.Email}",
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

        public async Task<LoginResponseDto?> LoginUser(LoginUserRequest loginUser)
        {
            var user = await _userRepository.GetUserWithEmail(loginUser.Email);
            if (user == null)
            {
                return null;
            }
            var passwordVerificationResult = _passwordHasher
                .VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var tokenOptions = _tokenGenerator.GetTokenOptions();

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, loginUser.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            var token = _tokenService.GenerateToken(claims, tokenOptions);

            return new LoginResponseDto()
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(tokenOptions.ExpiryMinutes),
            };
        }

        public async Task UpdateEmailConfirmation(Guid userId)
        {
            await _userRepository.UpdateEmailConfirmation(userId);
        }
    }
}
