using System.ComponentModel.DataAnnotations;

namespace TheScientist.Model
{
    public class Notification
    {
        [Key]
        public int ID { get; set; }

        public string Text { get; set; }

        public NotificationType NotificationType { get; set; }

        public List<NotificationUser> Users { get; set; }

        public Section Section { get; set; }
    }
}
