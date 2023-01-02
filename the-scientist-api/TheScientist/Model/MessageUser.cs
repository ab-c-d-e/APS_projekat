using System.ComponentModel.DataAnnotations;

namespace TheScientist.Model
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
