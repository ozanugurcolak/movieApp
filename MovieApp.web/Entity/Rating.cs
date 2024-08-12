using movieApp.web.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movieApp.web.Entity
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [Range(1, 10)]
        public int Score { get; set; }

        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
