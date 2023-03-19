using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public List<Section> Subsections { get; set; }
    }
}
