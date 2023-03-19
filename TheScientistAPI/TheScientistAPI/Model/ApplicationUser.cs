using Microsoft.AspNetCore.Identity;

namespace TheScientistAPI.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
