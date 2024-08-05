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
	}
	public class AdminCreateMovieModel
	{
		[Display(Name = "Film Adı")]
		[Required(ErrorMessage = "Film Adı Girmelisiniz")]
		public string Title { get; set; }

		[Display(Name = "Film Açıklaması")]
		[Required(ErrorMessage = "Film Açıklaması Girmelisiniz")]
		public string Description { get; set; }
		[Required(ErrorMessage = "en az bir tür Girmelisiniz")]

		public int[] GenreIds { get; set; }
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
		[Required(ErrorMessage = "en az bir tür Girmelisiniz")]

		public int[] GenreIds { get; set; }

	}
}
