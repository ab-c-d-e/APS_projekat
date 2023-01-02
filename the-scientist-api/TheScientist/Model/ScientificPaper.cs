using System.ComponentModel.DataAnnotations;

namespace TheScientist.Model
{
    public class ScientificPaper
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string Description { get; set; }

        public List<Section> Sections { get; set; }
    }
}
