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

        public Task<bool> LoginUser(LoginUserRequest loginUser);
    }
}
