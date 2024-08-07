using System.Linq;
using movieApp.web.Data;
using movieApp.web.Models;

namespace movieApp.web.Services
{
    public class AdminService : IAdminService
    {
        private readonly MovieContext _context;

        public AdminService(MovieContext context)
        {
            _context = context;
        }

        public Admin Authenticate(string username, string password)
        {
            return _context.Admins.SingleOrDefault(a => a.AdminUsername == username && a.Password == password);
        }
    }

    public interface IAdminService
    {
        Admin Authenticate(string username, string password);
    }
}
