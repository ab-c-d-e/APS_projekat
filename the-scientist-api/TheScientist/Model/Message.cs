using System.ComponentModel.DataAnnotations;

namespace TheScientist.Model
{
    public class Message
    {
        [Key]
        public int ID { get; set; }

        public string Text { get; set; }

        public User Senders { get; set; }

        public List<MessageUser> Recivers { get; set; }
    }
}
