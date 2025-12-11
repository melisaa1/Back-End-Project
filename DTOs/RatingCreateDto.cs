namespace RateNowApi.DTOs.Ratings
{
    public class RatingCreateDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int Score { get; set; } 
    }
}
