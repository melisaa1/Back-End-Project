using RateNowApi.Models;

namespace RateNowApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int userId);
    }
}