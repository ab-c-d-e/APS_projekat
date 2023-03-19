using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class Keyword
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
