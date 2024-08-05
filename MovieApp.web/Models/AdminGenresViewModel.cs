using movieApp.web.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace movieApp.web.Models
{
	public class AdminGenresViewModel
	{
		[Required(ErrorMessage ="tür bilgisi girmelisiniz")]
        public string Name { get; set; }
        public List<AdminGenreViewModel> Genres { get; set; }
	}

	public class AdminGenreViewModel
	{
		public int GenreId { get; set; }
		public string Name { get; set; }
		public int Count { get; set; }
	}

	public class AdminGenreEditViewModel
	{
		public int GenreId { get; set; }
        [Required(ErrorMessage = "tür bilgisi girmelisiniz")]
        public string Name { get; set; }
		public List<AdminMovieViewModel> Movies { get; set; }
	}
}
