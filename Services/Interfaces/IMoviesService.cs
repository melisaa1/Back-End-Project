using RateNowApi.Models;

namespace RateNowApi.Services.Interfaces
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        Task<Movie?> GetMovieByIdAsync(int id);

        Task<Movie> AddMovieAsync(Movie movie);

        Task<bool> UpdateMovieAsync(int id, Movie movie);

        Task<bool> DeleteMovieAsync(int id);
    }
}