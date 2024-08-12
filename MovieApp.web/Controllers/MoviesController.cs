using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using movieApp.web.Data;
using movieApp.web.Entity;
using movieApp.web.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieApp.web.Controllers
{

    public class MoviesController : Controller
    {
        private readonly MovieContext _context;
        public MoviesController(MovieContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
       
        [HttpGet]
        public IActionResult List(int? id, string q)
        {
            var movies = _context.Movies.AsQueryable();
            if (id != null)
            {
                movies = movies.Include(m => m.Genres).Where(m => m.Genres.Any(g => g.GenreId == id));
            }

            if (string.IsNullOrEmpty(q) || movies == null)
            {
            }
            else
            {
                movies = movies.Where(i =>
                    (i.Title != null && i.Title.ToLower().Contains(q.ToLower())) ||
                    (i.Description != null && i.Description.ToLower().Contains(q.ToLower()))
                );
            }
            var model = new MoviesViewModel()
            {
                Movies = movies.ToList()
            };

            return View("movies", model);
        }

     

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Casts)
                .ThenInclude(c => c.Person)
                .Include(m => m.Crews)
                .ThenInclude(c => c.Person)
                .Include(m => m.Ratings) 
                .FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            var averageRating = movie.Ratings.Any()
                ? movie.Ratings.Average(r => r.Score)
                : (double?)null;

            var viewModel = new AdminMovieViewModel
            {
                Movie = movie,
                Casts = movie.Casts.ToList(),
                Crews = movie.Crews.ToList(),
                AverageRating = averageRating ?? 0.0
            };

            return View(viewModel);
        }



        [HttpPost]
        public IActionResult RateMovie(int movieId, int score)
        {
            
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return RedirectToAction("Login", "UserAccount");
            }

            // puanlama kontrol
            var existingRating = _context.Ratings.FirstOrDefault(r => r.MovieId == movieId && r.UserId == user.UserId);

            if (existingRating != null)
            {
                // Mevcut puanı güncelle
                existingRating.Score = score;
            }
            else
            {
                // Yeni bir puanlama ekle
                var newRating = new Rating
                {
                    MovieId = movieId,
                    UserId = user.UserId,
                    Score = score,
                };
                _context.Ratings.Add(newRating);
            }

            _context.SaveChanges();

            // Ortalama hesaplama
            var averageRating = _context.Ratings.Where(r => r.MovieId == movieId).Average(r => r.Score);
            ViewBag.AverageRating = averageRating;

            return RedirectToAction("Details", new { id = movieId });
        }

    }
}