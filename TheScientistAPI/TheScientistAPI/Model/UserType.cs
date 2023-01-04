using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class UserType
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }
    }
}
