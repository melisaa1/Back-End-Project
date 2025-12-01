using RateNow.Models;

namespace RateNowApi.Models {
    public class Movie {
        public int Id { get; set; }              
        public string Title { get; set; } = null!; 

        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
