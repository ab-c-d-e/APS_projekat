using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheScientistAPI.Model
{
    public class MessageUser
    {
        [Key]
        public int ID { get; set; }

        public bool Seen { get; set; }

        public User User { get; set; }

        public Message Message { get; set; }
    }
}
