using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class Section:Segment
    {

        [MaxLength(50)]
        public string Title { get; set; }

        public int Number { get; set; }

        public int Depth { get; set; }

        public List<Segment>? Segments { get; set; }
    }
}
