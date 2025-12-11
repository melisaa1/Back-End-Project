namespace RateNowApi.DTOs.Movies
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }

        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
