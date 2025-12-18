using RateNowApi.Models;

namespace RateNowApi.Services.Interfaces
{
    public interface IRatingService
    {
        Task<IEnumerable<Rating>> GetAllRatingsAsync();
        Task<Rating?> GetRatingByIdAsync(int id);
        Task<Rating> AddRatingAsync(Rating rating);
        Task<bool> UpdateRatingAsync(int id, Rating rating);
        Task<bool> DeleteRatingAsync(int id);
    }
}