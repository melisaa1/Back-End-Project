using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;
using RateNowApi.Models;
using RateNowApi.Services;

namespace RateNowApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public AuthController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check unique email
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("A user with this email already exists.");

            var user = new User
            {
                UserName = request.Name,
                Email = request.Email,
                PasswordHash = _authService.HashPassword(request.Password),
                Role = request.Role ?? "User"  // default role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully", UserId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            bool passwordValid = _authService.VerifyPassword(request.Password, user.PasswordHash);

            if (!passwordValid)
                return Unauthorized("Invalid email or password.");

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

   

    public class RegisterRequest
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; } // optional (Admin, User…)
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}