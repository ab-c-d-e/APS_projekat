using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheScientistAPI.Model
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Url { get; set; }
        public string? Content { get; set; }
        public string? Language { get; set; }
        public SectionType Type { get; set; }
        public SectionStatus Status { get; set; }

        public ScientificPaper? Paper { get; set; }

        [JsonIgnore]
        public List<Section> Subsections { get; set; }
    }

    public enum SectionType
    {
        Text,
        Code, 
        Image
    }

    public enum SectionStatus
    {
        Active,
        InReview, 
        Closed, 
        Finnished
    }
}
