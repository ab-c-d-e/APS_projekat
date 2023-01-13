using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TheScientistAPI.Model
{
    public class Message
    {
        [Key]
        public int ID { get; set; }

        public string Text { get; set; }

        public User Sender { get; set; }

        public List<MessageUser>? Recivers { get; set; }

        public ScientificPaper ScientificPaper { get; set; }
    }
}
