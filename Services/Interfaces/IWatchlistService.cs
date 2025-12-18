using RateNowApi.Models;

namespace RateNowApi.Services.Interfaces
{
    public interface IWatchlistService
    {
        Task<List<WatchlistItem>> GetWatchlistAsync(int userId);

        Task<(bool Success, string Message, WatchlistItem? Item)>
            AddToWatchlistAsync(WatchlistItem item);

        Task<bool> UpdateWatchlistItemStatusAsync(int id, WatchlistItem item);

        Task<bool> RemoveFromWatchlistAsync(int id);
    }
}