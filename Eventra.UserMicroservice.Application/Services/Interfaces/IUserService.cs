using Eventra.UserMicroservice.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventra.UserMicroservice.Application.Services.Interfaces
{
    public interface IUserService
    {
        public Task<bool> RegisterUser(RegisterUserRequest registerUser);
        public Task<RegisterUserRequest> GetRegisterdUser(Guid userId);
        public Task UpdateEmailConfirmation(Guid userId);
        public Task<LoginResponseDto?> LoginUser(LoginUserRequest loginUser);
    }
}
