using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheScientistAPI.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [JsonIgnore]
        public List<UserRole> UserRoles { get; set; }

        [JsonIgnore]
        public List<MessageUser> Messages { get; set; }

        [NotMapped]
        public string? Name { get; set; }

    }
}
