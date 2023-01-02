using System.ComponentModel.DataAnnotations;

namespace TheScientist.Model
{
    public class UserType
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }
    }
}
