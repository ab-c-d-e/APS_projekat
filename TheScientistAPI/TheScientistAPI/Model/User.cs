using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheScientistAPI.Model
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        public string Password { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(30)]
        public string LastName { get; set; }

        public List<PaperUser> Papers { get; set; }

        public List<NotificationUser> Notifications { get; set; }

        public List<MessageUser> Messages { get; set; }

        public List<ToDoList> ToDoLists { get; set; }
    }
}
