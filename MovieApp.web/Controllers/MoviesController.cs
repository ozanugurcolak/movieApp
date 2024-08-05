﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using movieApp.web.Data;
using movieApp.web.Entity;
using movieApp.web.Models;
using SQLitePCL;
using System.Collections.Generic;
using System.Linq;

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
        //action
        // localhost:504040/movies/list/1
        [HttpGet]
        public IActionResult List(int? id,string q)
        {
            //var movies = MovieRepository.Movies;
            var movies= _context.Movies.AsQueryable();
            if (id != null)
            { 
                movies = movies.Include(m=>m.Genres).Where(m => m.Genres.Any(g=>g.GenreId==id));
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

            return View("movies",model);
        }

        [HttpGet]
        public IActionResult Details(int id) 
        {
            return View(_context.Movies.Find(id));
        }
       
	}
}
