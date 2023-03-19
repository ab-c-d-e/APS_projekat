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
        public bool IsPublic { get; set; }
        public bool IsFinished { get; set; }

        public ApplicationUser Creator { get; set; }

        public List<Section> Sections { get; set; }
        public List<Keyword> Keywords { get; set; }
        public List<Reference> References { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
