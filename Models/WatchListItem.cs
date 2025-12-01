namespace RateNowApi.Models {
    public class WatchlistItem {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }

        public string Status { get; set; } = "Planned"; // Planned / Watching / Completed
    }
}
