using RateNowApi.Models;

namespace RateNowApi.Services.Interfaces
{
    public interface IWatchListService
    {
        Task<List<WatchListItem>> GetWatchListAsync(int userId);

        Task<(bool Success, string Message, WatchListItem? Item)>
            AddToWatchListAsync(WatchListItem item);

        Task<bool> UpdateWatchListItemStatusAsync(int id, WatchListItem item);

        Task<bool> RemoveFromWatchListAsync(int id);
    }
}