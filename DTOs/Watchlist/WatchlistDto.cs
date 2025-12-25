namespace RateNowApi.DTOs.WatchList
{
    public class WatchListItemDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public String? Status { get; set; }
    }
}
