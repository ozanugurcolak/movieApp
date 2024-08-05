using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;

namespace movieApp.web.Entity
{
	public class Movie
	{
		public Movie() 
		{
			Genres = new List<Genre>();
		}
		[Key]
		public int MovieId { get; set; }
		[Required]
		public string Title { get; set; }

		public string Description { get; set; }
		public string ImageUrl { get; set; }		

        public List<Genre> Genres { get; set; } 
    }
}
