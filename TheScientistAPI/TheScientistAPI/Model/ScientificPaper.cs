using System.ComponentModel.DataAnnotations;
using TheScientistAPI.Model;

namespace TheScientistAPI.Model
{
    public class ScientificPaper
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }

        public List<Segment>? Segments { get; set; }
    }
}
