using System.ComponentModel.DataAnnotations;

namespace movieApp.web.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [StringLength(50)]
        public string AdminUsername { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
