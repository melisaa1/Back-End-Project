using Microsoft.AspNetCore.Mvc;
using RateNowApi.DTOs;
using RateNowApi.DTOs.Users;
using RateNowApi.Models;
using RateNowApi.Services.Interfaces;

namespace RateNowApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserService userService,
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _authService = authService;
            _logger = logger;
        }

        // REGISTER
      
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            _logger.LogInformation("Register attempt for email: {Email}", request.Email);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email exists
            bool emailExists = await _userService.EmailExistsAsync(request.Email);
            if (emailExists)
            {
                _logger.LogWarning("Registration failed: Email already exists -> {Email}", request.Email);
                return BadRequest("A user with this email already exists.");
            }

            // Create User model (mapping DTO -> Model)
            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                PasswordHash = _authService.HashPassword(request.Password),
                Role = request.Role ?? "User"
            };

            await _userService.CreateUserAsync(user);

            _logger.LogInformation("User registered successfully: {Email}", request.Email);

            // Response DTO
            var response = new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };

            return Ok(response);
        }



        // LOGIN
   
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found -> {Email}", request.Email);
                return Unauthorized("Invalid email or password.");
            }

            bool validPassword =
                _authService.VerifyPassword(request.Password, user.PasswordHash);

            if (!validPassword)
            {
                _logger.LogWarning("Login failed: Invalid password -> {Email}", request.Email);
                return Unauthorized("Invalid email or password.");
            }

            string token = _authService.GenerateJwtToken(user);

            _logger.LogInformation("Login successful: {Email}", request.Email);

           
            var response = new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };

            return Ok(response);
        }

        
    }
}
