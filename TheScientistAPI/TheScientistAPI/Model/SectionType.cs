using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class SectionType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
