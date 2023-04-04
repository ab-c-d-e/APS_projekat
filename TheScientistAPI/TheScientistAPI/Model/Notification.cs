using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheScientistAPI.Model
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        [JsonIgnore]
        public NotificationType Type { get; set; }
        public bool Seen { get; set; }

        [JsonIgnore]
        public PaperTask Task { get; set; }

        [JsonIgnore]
        public Comment Comment { get; set; }
    }
    
    public enum NotificationType
    {
        Task,
        Comment
    }
}
