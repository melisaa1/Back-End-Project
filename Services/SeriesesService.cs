using Microsoft.EntityFrameworkCore;
using RateNowApi.Data;
using RateNowApi.Models;
using RateNowApi.Services.Interfaces;

namespace RateNowApi.Services
{
    public class SeriesesService : ISeriesesService
    {
        private readonly AppDbContext _context;

        public SeriesesService(AppDbContext context)
        {
            _context = context;
        }
//get all series
        public async Task<List<Series>> GetAllSeriesAsync()
        {
            return await _context.Serieses.ToListAsync();
        }
//get series by id
        public async Task<Series?> GetSeriesByIdAsync(int id)
        {
            return await _context.Serieses.FirstOrDefaultAsync(s => s.Id == id);
        }
//add new series
        public async Task<Series> AddSeriesAsync(Series series)
        {
            _context.Serieses.Add(series);
            await _context.SaveChangesAsync();
            return series;
        }
//update series
        public async Task<bool> UpdateSeriesAsync(int id, Series updatedSeries)
        {
            var series = await _context.Serieses.FindAsync(id);
            if (series == null)
                return false;

            series.Title = updatedSeries.Title;
            series. Seasons = updatedSeries. Seasons;

            await _context.SaveChangesAsync();
            return true;
        }
//delete series
        public async Task<bool> DeleteSeriesAsync(int id)
        {
            var series = await _context.Serieses.FindAsync(id);
            if (series == null)
                return false;

            _context.Serieses.Remove(series);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
