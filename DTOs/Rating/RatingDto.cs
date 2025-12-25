namespace RateNowApi.DTOs.Ratings
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
    }
}
