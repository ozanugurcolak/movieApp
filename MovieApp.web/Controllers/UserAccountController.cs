using Microsoft.AspNetCore.Mvc;
using movieApp.web.Data;
using movieApp.web.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace movieApp.web.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly MovieContext _context;

        public UserAccountController(MovieContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(User user)
        {
            // Kullanıcı adı veya email kontrolü
            var existingUser = _context.Users
                                       .FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email);

            if (existingUser != null)
            {
                if (existingUser.Username == user.Username)
                {
                    ModelState.AddModelError("Username", "Bu kullanıcı adı zaten alınmış.");
                }

                if (existingUser.Email == user.Email)
                {
                    ModelState.AddModelError("Email", "Bu email adresi zaten kayıtlı.");
                }

                // Hataları kullanıcıya gösterin
                return View(user);
            }

            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login", "UserAccount");
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("UserLogin"); // UserLogin.cshtml dosyasını döndür
        }

        // POST: /UserAccount/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre.";
            return View("UserLogin"); // Hatalı girişte UserLogin sayfasını döndür
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Account()
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddToWatchlist(int movieId)
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var watchlistEntry = new Watchlist
            {
                UserId = user.UserId,
                MovieId = movieId
            };

            _context.Watchlists.Add(watchlistEntry);
            _context.SaveChanges();

            TempData["SuccessMessage"] = HttpUtility.HtmlEncode("Film başarılı bir şekilde izleme listesine eklendi.");


            return RedirectToAction("List","Movies");
        }

        // İzleme Listesini Görüntüleme
        [HttpGet]
        public IActionResult Watchlist()
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var watchlist = _context.Watchlists
                                    .Where(w => w.UserId == user.UserId)
                                    .Select(w => w.Movie)
                                    .ToList();

            return View(watchlist);
        }
        [HttpPost]
        public IActionResult RemoveFromWatchlist(int movieId)
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var watchlistEntry = _context.Watchlists
                                         .FirstOrDefault(w => w.UserId == user.UserId && w.MovieId == movieId);

            if (watchlistEntry != null)
            {
                _context.Watchlists.Remove(watchlistEntry);
                _context.SaveChanges();
            }

            return RedirectToAction("Watchlist");
        }
    }
}
