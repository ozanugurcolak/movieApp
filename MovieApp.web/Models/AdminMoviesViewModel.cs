using movieApp.web.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace movieApp.web.Models
{
	public class AdminMoviesViewModel
	{
		public List <AdminMovieViewModel> Movies { get; set; }
	}
	public class AdminMovieViewModel
	{
		public int MovieId { get; set; }
		public string Title { get; set; }
		public string ImageUrl { get; set; }
		public List<Genre> Genres { get; set; }
        public Movie Movie { get; set; }
        public List<Cast> Casts { get; set; }
        public List<Crew> Crews { get; set; }
        public Person Director { get; set; } // Yönetmen bilgisi
        public List<Person> Actors { get; set; } // Oyuncular bilgisi
    }
    public class AdminCreateMovieModel
    {
        [Display(Name = "Film Adı")]
        [Required(ErrorMessage = "Film Adı Girmelisiniz")]
        public string Title { get; set; }

        [Display(Name = "Film Açıklaması")]
        [Required(ErrorMessage = "Film Açıklaması Girmelisiniz")]
        public string Description { get; set; }

        [Display(Name = "Görsel URL")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "En az bir tür Girmelisiniz")]
        public int[] GenreIds { get; set; }

        [Display(Name = "Yönetmen")]
        public string DirectorName { get; set; }

        [Display(Name = "Oyuncular (virgülle ayrılmış)")]
        public string ActorNames { get; set; }
    }
    public class AdminEditMovieViewModel
    {
        public int MovieId { get; set; }

        [Display(Name = "Film Adı")]
        [Required(ErrorMessage = "Film Adı Girmelisiniz")]
        public string Title { get; set; }

        [Display(Name = "Film Açıklaması")]
        [Required(ErrorMessage = "Film Açıklaması Girmelisiniz")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "En az bir tür Girmelisiniz")]
        public int[] GenreIds { get; set; }

        public List<Genre> Genres { get; set; } = new List<Genre>();

        public List<Cast> Casts { get; set; } = new List<Cast>();

        public List<Crew> Crews { get; set; } = new List<Crew>();

        [Display(Name = "Yönetmen")]
        public string DirectorName { get; set; }

        [Display(Name = "Oyuncular (virgülle ayrılmış)")]
        public string ActorNames { get; set; }
    }


    public class CastViewModel
    {
        public int CastId { get; set; }
        public string Name { get; set; }
    }

    public class CrewViewModel
    {
        public int CrewId { get; set; }
        public string Job { get; set; }
    }

}
