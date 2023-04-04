using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheScientistAPI.Model
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
