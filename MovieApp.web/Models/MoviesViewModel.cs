using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using movieApp.web.Entity;
using System.ComponentModel.DataAnnotations;

namespace movieApp.web.Models
{
    public class MoviesViewModel
    {
        public List<Movie> Movies { get; set; }
    }
    public class WatchlistModel
    {
        public int MovieId { get; set; }
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<Cast> Casts { get; set; }
        public List<Crew> Crews { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Movie> Movies { get; set; }

    }

}