using System.ComponentModel.DataAnnotations;

namespace TheScientist.Model
{
    public class NotificationType
    {
        [Key]
        public int ID { get; set; }

        public string Type { get; set; }
    }
}
