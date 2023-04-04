using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace TheScientistAPI.Model
{
    public class ScientificPaper
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Journal { get; set; }
        public bool IsPublic { get; set; }
        public int Year { get; set; }
        public PaperStatus Status { get; set; }
        [JsonIgnore]
        public ApplicationUser Creator { get; set; }
        [JsonIgnore]
        public List<Section> Sections { get; set; }
        [JsonIgnore]
        public List<Keyword> Keywords { get; set; }
        [JsonIgnore]
        public List<Reference> References { get; set; }
        [JsonIgnore]
        public List<UserRole> UserRoles { get; set; }
        [JsonIgnore]
        public List<Message> Messages { get; set; }
    }

    public enum PaperStatus
    {
        Active, 
        Closed, 
        InReview,
        Published
    }
}
