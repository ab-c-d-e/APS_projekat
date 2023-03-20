using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public SectionType Type { get; set; }
        public ScientificPaper Paper { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public List<Section> Subsections { get; set; }
    }

    public enum SectionType
    {
        Text,
        Code, 
        Image
    }
}
