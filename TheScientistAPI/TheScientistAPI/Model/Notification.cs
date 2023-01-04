using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class Notification
    {
        [Key]
        public int ID { get; set; }

        public string Text { get; set; }

        public NotificationType NotificationType { get; set; }

        public List<NotificationUser> Users { get; set; }

        public Segment Segment { get; set; }
    }
}
