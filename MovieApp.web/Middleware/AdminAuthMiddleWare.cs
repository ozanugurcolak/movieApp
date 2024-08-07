using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace movieApp.web.Middleware
{
    public class AdminAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();

            // Admin gerektiren sayfaları kontrol et
            if (path.StartsWith("/admin") || path.StartsWith("/moviecreate") || path.StartsWith("/movieupdate") || path.StartsWith("/genrelist"))
            {
                // Admin oturum kontrolü
                if (!context.User.Identity.IsAuthenticated || !context.User.IsInRole("Admin"))
                {
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}
