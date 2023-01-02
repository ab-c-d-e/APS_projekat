using System.ComponentModel.DataAnnotations;

namespace TheScientist.Model
{
    public class Section
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public int Depth { get; set; }

        public List<Section> Sections { get; set; } 

        public Decorator Decorator { get; set;  }
    }
}
