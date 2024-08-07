﻿
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movieApp.web.Data;
using movieApp.web.Entity;
using movieApp.web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace movieApp.web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        private readonly MovieContext _context;
        public AdminController(MovieContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var admin = _context.Admins.SingleOrDefault(a => a.AdminUsername == username && a.Password == password);
            if (admin != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.AdminUsername),
                new Claim(ClaimTypes.Role, "Admin")
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false 
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Admin");
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MovieUpdate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Casts)
                .ThenInclude(c => c.Person) // Person nesnelerini de dahil ediyoruz
                .Include(m => m.Crews)
                .ThenInclude(c => c.Person) // Person nesnelerini de dahil ediyoruz
                .FirstOrDefault(m => m.MovieId == id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new AdminEditMovieViewModel
            {
                MovieId = entity.MovieId,
                Title = entity.Title,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                GenreIds = entity.Genres?.Select(i => i.GenreId).ToArray() ?? new int[0], // Null check
                Genres = _context.Genres.ToList(),
                Casts = entity.Casts?.ToList() ?? new List<Cast>(), // Null check
                Crews = entity.Crews?.ToList() ?? new List<Crew>(), // Null check
                DirectorName = entity.Crews?.FirstOrDefault(c => c.Job == "Director")?.Person?.Name, // Null check
                ActorNames = entity.Casts != null ? string.Join(", ", entity.Casts.Where(c => c.Person != null).Select(c => c.Person.Name)) : string.Empty // Null check
            };

            ViewBag.Genres = _context.Genres.ToList();

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> MovieUpdate(AdminEditMovieViewModel model, int[] genreIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = await _context.Movies
                    .Include(m => m.Genres)
                    .Include(m => m.Crews)
                    .Include(m => m.Casts)
                    .FirstOrDefaultAsync(m => m.MovieId == model.MovieId);

                if (entity == null)
                {
                    return NotFound();
                }

                entity.Title = model.Title;
                entity.Description = model.Description;

                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", fileName);
                    entity.ImageUrl = fileName;

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                entity.Genres = genreIds.Select(id => _context.Genres.FirstOrDefault(i => i.GenreId == id)).ToList();

                // Yönetmen ve oyuncuları güncelle
                if (!string.IsNullOrEmpty(model.DirectorName))
                {
                    var director = _context.People.FirstOrDefault(p => p.Name == model.DirectorName);
                    if (director == null)
                    {
                        director = new Person { Name = model.DirectorName };
                        _context.People.Add(director);
                        await _context.SaveChangesAsync();
                    }
                    var crew = entity.Crews.FirstOrDefault(c => c.Job == "Director");
                    if (crew == null)
                    {
                        crew = new Crew { MovieId = entity.MovieId, PersonId = director.PersonId, Job = "Director" };
                        _context.Crews.Add(crew);
                    }
                    else
                    {
                        crew.PersonId = director.PersonId;
                    }
                }

                entity.Casts.Clear();
                if (!string.IsNullOrEmpty(model.ActorNames))
                {
                    var actorNames = model.ActorNames.Split(',').Select(name => name.Trim()).ToList();
                    foreach (var actorName in actorNames)
                    {
                        var actor = _context.People.FirstOrDefault(p => p.Name == actorName);
                        if (actor == null)
                        {
                            actor = new Person { Name = actorName };
                            _context.People.Add(actor);
                            await _context.SaveChangesAsync();
                        }
                        entity.Casts.Add(new Cast { MovieId = entity.MovieId, PersonId = actor.PersonId, Name = actorName });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("MovieList");
            }

            ViewBag.Genres = _context.Genres.ToList();
            return View(model);
        }


        public IActionResult MovieList()
        {
            return View(new AdminMoviesViewModel
            {
                Movies = _context.Movies
                .Include(m => m.Genres)
                .Select(m => new AdminMovieViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    ImageUrl = m.ImageUrl,
                    Genres = m.Genres.ToList()
                })
                .ToList()
            });

        }

        public IActionResult GenreList()
        {
            return View(GetGenres());
        }

        private AdminGenresViewModel GetGenres()
        {
            return new AdminGenresViewModel
            {
                Genres = _context.Genres.Select(g => new AdminGenreViewModel
                {
                    GenreId = g.GenreId,
                    Name = g.Name,
                    Count = g.Movies.Count
                }).ToList()

            };
        }

        [HttpPost]
        public IActionResult GenreCreate(AdminGenresViewModel model)
        {
            if (model.Name != null && model.Name.Length < 3)
            {
                ModelState.AddModelError("Name", "tür adı minimum 3 karakterli olmalıdır");
            }
            if (ModelState.IsValid)
            {
                _context.Genres.Add(new Genre { Name = model.Name });
                _context.SaveChanges();
                return RedirectToAction("GenreList");
            }
            return View("GenreList", GetGenres());
        }

        public IActionResult GenreUpdate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _context
                .Genres
                .Select(g => new AdminGenreEditViewModel
                {
                    GenreId = g.GenreId,
                    Name = g.Name,
                    Movies = g.Movies.Select(i => new AdminMovieViewModel
                    {
                        MovieId = i.MovieId,
                        Title = i.Title,
                        ImageUrl = i.ImageUrl,

                    }).ToList()
                }).FirstOrDefault(g => g.GenreId == id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost]
        public IActionResult GenreUpdate(AdminGenreEditViewModel model, int[] movieIds)
        {
            if (ModelState.IsValid)
            {

                var entity = _context.Genres.Include("Movies").FirstOrDefault(i => i.GenreId == model.GenreId);
                if (entity == null) { return NotFound(); }
                entity.Name = model.Name;
                foreach (var id in movieIds) { entity.Movies.Remove(entity.Movies.FirstOrDefault(m => m.MovieId == id)); }
                _context.SaveChanges();
                return RedirectToAction("GenreList");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult GenreDelete(int genreId)
        {
            var entity = _context.Genres.Find(genreId);
            if (entity != null)
            {
                _context.Genres.Remove(entity);
                _context.SaveChanges();
            }
            return RedirectToAction("GenreList");
        }
        [HttpPost]
        public IActionResult MovieDelete(int movieId)
        {
            var entity = _context.Movies.Find(movieId);
            if (entity != null)
            {
                _context.Movies.Remove(entity);
                _context.SaveChanges();
            }
            return RedirectToAction("MovieList");
        }

        public IActionResult MovieCreate()
        {
            ViewBag.Genres = _context.Genres.ToList();
            return View(new AdminCreateMovieModel());
        }

        //	[HttpPost]
        //public IActionResult MovieCreate(Movie m, int[] genreIds)
        //{
        //if (ModelState.IsValid)
        //{
        //m.Genres = new List<Genre>();
        //foreach(var id in genreIds)
        //{
        //	m.Genres.Add(_context.Genres.FirstOrDefault(i=> i.GenreId == id));	
        //}
        //_context.Movies.Add(m);
        //_context.SaveChanges();
        //return RedirectToAction("MovieList","Admin");
        //}
        //ViewBag.Genres = _context.Genres.ToList();
        //return View();
        //}

        [HttpPost]
        public async Task<IActionResult> MovieCreate(AdminCreateMovieModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = new Movie
                {
                    Title = model.Title,
                    Description = model.Description,
                    ImageUrl = "no-image.png"
                };

                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var fileName = string.Format($"{Guid.NewGuid()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", fileName);
                    entity.ImageUrl = fileName;

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                // Movie ekle
                _context.Movies.Add(entity);
                await _context.SaveChangesAsync();

                // Genre'leri ekle
                foreach (var id in model.GenreIds)
                {
                    entity.Genres.Add(_context.Genres.FirstOrDefault(i => i.GenreId == id));
                }

                // Yönetmen ekle
                if (!string.IsNullOrEmpty(model.DirectorName))
                {
                    var director = new Person { Name = model.DirectorName };
                    _context.People.Add(director);
                    await _context.SaveChangesAsync();
                    var crew = new Crew { MovieId = entity.MovieId, PersonId = director.PersonId, Job = "Director" };
                    _context.Crews.Add(crew);
                }

                // Oyuncuları ekle
                if (!string.IsNullOrEmpty(model.ActorNames))
                {
                    var actorNames = model.ActorNames.Split(',').Select(name => name.Trim()).ToList();
                    foreach (var actorName in actorNames)
                    {
                        var actor = new Person { Name = actorName };
                        _context.People.Add(actor);
                        await _context.SaveChangesAsync();
                        var cast = new Cast { MovieId = entity.MovieId, PersonId = actor.PersonId, Name = actorName };
                        _context.Casts.Add(cast);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("MovieList", "Admin");
            }

            ViewBag.Genres = _context.Genres.ToList();
            return View(model);
        }




    }
}
