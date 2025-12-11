namespace RateNowApi.DTOs.Watchlist
{
    public class WatchlistItemDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public bool IsWatched { get; set; }
    }
}
