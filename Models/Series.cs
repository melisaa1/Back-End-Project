using RateNow.Models;

namespace RateNowApi.Models {
    public class Series {
        public int Id { get; set; }
        public string? Title { get; set; } = null!;
        public int Seasons { get; set; }

      public ICollection<Rating>? Ratings { get; set; }
      public ICollection<Review>? Reviews { get; set; }

    }
}
