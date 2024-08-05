using Microsoft.AspNetCore.Mvc;
using movieApp.web.Models;

namespace movieApp.web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateUser(UserModel model)
        {
            return View();
        }
    }
}
