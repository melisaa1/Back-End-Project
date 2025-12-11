using Microsoft.AspNetCore.Mvc;
using RateNowApi.Models;
using RateNowApi.Services;

namespace RateNowApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserService userService,
            AuthService authService,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _authService = authService;
            _logger = logger;
        }

        // ----------------------------------------------------
        // REGISTER
        // ----------------------------------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            _logger.LogInformation("Register attempt for email: {Email}", request.Email);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check unique email
            if (await _userService.EmailExistsAsync(request.Email))
            {
                _logger.LogWarning("Registration failed: Email already exists -> {Email}", request.Email);
                return BadRequest("A user with this email already exists.");
            }

            // Create new user
            var user = new User
            {
                UserName = request.Name,
                Email = request.Email,
                PasswordHash = _authService.HashPassword(request.Password),
                Role = request.Role ?? "User"
            };

            await _userService.CreateUserAsync(user);

            _logger.LogInformation("User registered successfully: {Email}", request.Email);

            return Ok(new
            {
                Message = "User registered successfully",
                UserId = user.Id
            });
        }

        // ----------------------------------------------------
        // LOGIN
        // ----------------------------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
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

            bool validPassword = _authService.VerifyPassword(request.Password, user.PasswordHash);

            if (!validPassword)
            {
                _logger.LogWarning("Login failed: Invalid password for -> {Email}", request.Email);
                return Unauthorized("Invalid email or password.");
            }

            _logger.LogInformation("Login successful: {Email}", request.Email);

            string token = _authService.GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                User = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.Role
                }
            });
        }
    }

    // ----------------------------------------------------
    // REQUEST MODELS
    // ----------------------------------------------------
    public class RegisterRequest
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}