using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheScientistAPI.Model;

namespace TheScientistAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected async Task<ApplicationUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
