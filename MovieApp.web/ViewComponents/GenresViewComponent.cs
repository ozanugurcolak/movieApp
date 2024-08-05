using Microsoft.AspNetCore.Mvc;
using movieApp.web.Controllers;
using movieApp.web.Data;
using movieApp.web.Models;
using System.Collections.Generic;
using System.Linq;

namespace movieApp.web.ViewComponents
{

    public class GenresViewComponent : ViewComponent
    {
        private readonly MovieContext _context;
        public GenresViewComponent(MovieContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedGenre = RouteData.Values["id"];
            return View(_context.Genres.ToList());
        }
    }
}
