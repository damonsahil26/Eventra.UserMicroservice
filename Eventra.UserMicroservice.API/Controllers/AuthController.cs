using Eventra.UserMicroservice.Application.DTO;
using Eventra.UserMicroservice.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventra.UserMicroservice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthController(IUserService userService,
            ITokenService tokenService,
            IConfiguration config)
        {
            _userService = userService;
            _tokenService = tokenService;
            _config = config;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest loginUser)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(_ => _.Errors)
                    .ToList();

                return BadRequest(errors);
            }

            var loginResult = await _userService.LoginUser(loginUser);

            if(loginResult == null)
            {
                return Unauthorized();
            }

            return Ok(loginResult);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmUserRegistration(string token, string email)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token required");
            }

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email required");
            }

            var jwtSection = _config.GetRequiredSection("Jwt");

            var tokenOptions = new TokenOptionsDto
            {
                Issuer = jwtSection.GetRequiredSection("Issuer").Value ?? string.Empty,
                Audience = jwtSection.GetRequiredSection("Audience").Value ?? string.Empty
            };

            var claimsPrincipal = _tokenService.ValidateToken(token, tokenOptions);

            if (claimsPrincipal == null)
            {
                return BadRequest("Invalid or expired token");
            }

            var claimsUserId = claimsPrincipal.FindFirst("UserId")?.Value;
            var claimsEmail = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            if (claimsEmail == null 
                || claimsEmail.Equals(email, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Token email is not valid");
            }

            var userId = Guid.Parse(claimsUserId ?? string.Empty);
            var expiresAt = DateTime.Parse(claimsPrincipal.FindFirst("Expiry")?.Value
                ?? DateTime.MinValue.ToString());

            await _userService.UpdateEmailConfirmation(userId);

            return Ok("Email Verification Successful");
        }
    }
}
