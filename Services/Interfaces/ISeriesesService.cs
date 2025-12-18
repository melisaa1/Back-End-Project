using RateNowApi.Models;

namespace RateNowApi.Services.Interfaces
{
    public interface ISeriesesService
    {
        Task<List<Series>> GetAllSeriesAsync();
        Task<Series?> GetSeriesByIdAsync(int id);
        Task<Series> AddSeriesAsync(Series series);
        Task<bool> UpdateSeriesAsync(int id, Series updatedSeries);
        Task<bool> DeleteSeriesAsync(int id);
    }
}
