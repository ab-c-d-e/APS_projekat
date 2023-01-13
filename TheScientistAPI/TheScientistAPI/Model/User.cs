using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace TheScientistAPI.Model
{
    public class User:IdentityUser
    {

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(30)]
        public string? LastName { get; set; }

        public List<PaperUser>? Papers { get; set; }

        public List<NotificationUser>? Notifications { get; set; }

        public List<MessageUser>? Messages { get; set; }
    }
}
