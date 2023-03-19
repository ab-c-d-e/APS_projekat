using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class Reference
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Journal { get; set; }
        public int Year { get; set; }
    }
}
