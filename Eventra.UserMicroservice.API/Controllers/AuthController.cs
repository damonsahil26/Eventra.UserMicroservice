using Eventra.UserMicroservice.Application.DTO;
using Eventra.UserMicroservice.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Eventra.UserMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
                return BadRequest(errors);
            }

            var status = await _userService.LoginUser(loginRequest);

            if (!status)
            {
                return BadRequest("Invalid email or password.");
            }

            return Ok("Login successful");

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest registerUser)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors).ToList();
                return BadRequest(errors);
            }

            var status = await _userService.RegisterUser(registerUser);
            if (!status)
            {
                return BadRequest("User creation failed. User already exists");
            }

            return Ok("User created successfully!");
        }
    }
}
