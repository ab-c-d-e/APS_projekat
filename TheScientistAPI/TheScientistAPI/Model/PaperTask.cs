using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheScientistAPI.Model
{
    public class PaperTask
    {
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public Section Section { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        public string Description { get; set; }
        public SectionTaskStatus Status { get; set; }
    }

    public enum SectionTaskStatus
    {
        Active,
        InReview,
        Finnished
    }
}
