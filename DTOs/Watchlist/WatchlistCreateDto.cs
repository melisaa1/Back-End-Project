namespace RateNowApi.DTOs.WatchList
{
    public class WatchListCreateDto
    {
        public int UserId { get; set; }
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }

        public string? Status { get; set; }
    }
}
