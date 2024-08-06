using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using movieApp.web.Entity;
using System.Collections.Generic;
using System.Linq;

namespace movieApp.web.Data
{
	public static class DataSeeding
	{
		public static void seed(IApplicationBuilder app)
		{
			var scope = app.ApplicationServices.CreateScope();
			var context = scope.ServiceProvider.GetService<MovieContext>();

			context.Database.Migrate();
			var genres = new List<Genre>()
			{


						new Genre {Name="Macera", Movies=new List<Movie>()
							{
								new Movie { Title="yeni macera filmi 1",
								Description="A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O., but his tragic past may doom the project and his team to disaster.",
								ImageUrl="inception.jpg",
								},
								new Movie { Title="yeni macera filmi 2",
								Description="In Nazi-occupied France during World War II, a plan to assassinate Nazi leaders by a group of Jewish U.S. soldiers coincides with a theatre owner's vengeful plans for the same.",
								ImageUrl="inglourious.jpg",
								},
							}
						},
						new Genre {Name="Komedi"},
						new Genre {Name="Romantik"},
						new Genre {Name="Savaş"},
						new Genre {Name="Bilim Kurgu"}


			};
			var movies= new List<Movie>()
			{
				new Movie { Title="Inception",
					Description="A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O., but his tragic past may doom the project and his team to disaster.",
					ImageUrl="inception.jpg",
					Genres = new List<Genre>() {genres[0], new Genre() {Name="Yeni Tür" }, genres[1] } },
				new Movie { Title="Inglorious Bastards",
					Description="In Nazi-occupied France during World War II, a plan to assassinate Nazi leaders by a group of Jewish U.S. soldiers coincides with a theatre owner's vengeful plans for the same.",  
					ImageUrl="inglourious.jpg",
					Genres = new List<Genre>() {genres[0],genres[2] } },
				new Movie { Title="Interstellar",
					Description="When Earth becomes uninhabitable in the future, a farmer and ex-NASA pilot, Joseph Cooper, is tasked to pilot a spacecraft, along with a team of researchers, to find a new planet for humans.",  
					ImageUrl="interstellar.jpg" ,
					Genres = new List<Genre>() {genres[1],genres[3] } },
				new Movie { Title="Departed",
					Description="An undercover cop and a mole in the police attempt to identify each other while infiltrating an Irish gang in South Boston.",  
					ImageUrl="departed.jpg" ,
					Genres = new List<Genre>() {genres[0],genres[1] } },
				new Movie { Title="Matrix",
					Description="When a beautiful stranger leads computer hacker Neo to a forbidding underworld, he discovers the shocking truth--the life he knows is the elaborate deception of an evil cyber-intelligence.", 
					ImageUrl="matrix.jpg" ,
					Genres = new List<Genre>() {genres[2],genres[4] } },
				new Movie { Title = "Se7en",
					Description = "Two detectives, a rookie and a veteran, hunt a serial killer who uses the seven deadly sins as his motives.",
					ImageUrl = "seven.jpg",
					Genres = new List<Genre>() {genres[1],genres[2] } },
			};
			
			var people = new List<Person>()
			{
				new Person()
				{
					Name = "Personel1",
				
				},
				new Person()
				{
					Name = "Personel2",
					
				}
			};
			var crews = new List<Crew>()
			{
				new Crew() {Movie = movies[0], Person=people[0],Job="Yönetmen"},
				new Crew() {Movie = movies[0], Person=people[1],Job="Yönetmen Yard."},
			};
			var casts = new List<Cast>()
			{
				new Cast() {Movie=movies[0],Person=people[0],Name="Oyuncu 1"},
				new Cast() {Movie=movies[0],Person=people[1],Name="Oyuncu 2"}
			};


			if (context.Database.GetPendingMigrations().Count() == 0)
			{
				if (context.Genres.Count() == 0)
				{
					context.Genres.AddRange(genres);
				}
				if (context.Movies.Count() == 0) 
				{
					context.Movies.AddRange(movies);
				}
				
				if (context.People.Count() == 0)
				{
					context.People.AddRange(people);
				}
				if (context.Casts.Count() == 0)
				{
					context.Casts.AddRange(casts);
				}
				if (context.Crews.Count() == 0)
				{
					context.Crews.AddRange(crews);
				}

				context.SaveChanges();
			}
		}
	}
}
