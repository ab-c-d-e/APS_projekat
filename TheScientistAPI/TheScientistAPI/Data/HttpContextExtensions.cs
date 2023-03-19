using Microsoft.AspNetCore.Identity;
using TheScientistAPI.Model;

namespace TheScientistAPI.Data
{
    public static class HttpContextExtensions
    {
        public static async Task<ApplicationUser> GetUserAsync(this HttpContext httpContext)
        {
            var userManager = httpContext.RequestServices.GetService<UserManager<ApplicationUser>>();
            var user = await userManager.GetUserAsync(httpContext.User);

            return user;
        }
    }
}
