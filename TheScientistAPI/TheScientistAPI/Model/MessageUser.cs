using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class MessageUser
    {
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public Message Message { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get;set; }
        public bool Seen { get; set; }
    }
}
