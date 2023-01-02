using System.ComponentModel.DataAnnotations;

namespace TheScientist.Model
{
    public class NotificationUser
    {
        [Key]
        public int ID { get; set; }

        public bool Seen { get; set; }

        public User User { get; set; }

        public Notification Notification { get; set;}
    }
}
