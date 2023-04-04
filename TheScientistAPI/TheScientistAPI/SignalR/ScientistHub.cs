using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheScientistAPI.Model;

namespace TheScientistAPI.SignalR
{
    public class ScientistHub:Hub
    {
        
        private readonly UserManager<ApplicationUser> _userManager;

        public ScientistHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            string userId = Context.GetHttpContext().Request.Query["userId"];
            var user = await _userManager.Users.Include(u => u.UserRoles)
                .ThenInclude(uR => uR.ScientificPaper)
                .FirstOrDefaultAsync(u => u.Email == userId);
            foreach (var group in user.UserRoles)
                await Groups.AddToGroupAsync(Context.ConnectionId, group.ScientificPaper.Id.ToString());
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
    }
}
