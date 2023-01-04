using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public abstract class Segment
    {
        [Key]
        public int ID { get; set; }
    }
}
