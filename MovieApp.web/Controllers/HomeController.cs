using Microsoft.AspNetCore.Mvc;
using movieApp.web.Data;
using movieApp.web.Models;
using System.Collections.Generic;
using System.Linq;

namespace movieApp.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieContext _context;
        public HomeController (MovieContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = new HomePageViewModel
            {
                PopularMovies = _context.Movies.ToList()
            };
            
            return View(model);
        }
        public IActionResult About()
        {
           
            return View();
        }
    }
}
