using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class NotificationType
    {
        [Key]
        public int ID { get; set; }

        public string Type { get; set; }
    }
}
