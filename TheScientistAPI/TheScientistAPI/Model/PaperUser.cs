using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class PaperUser
    {
        [Key]
        public int ID { get; set; }

        public UserType UserType { get; set; }

        public ScientificPaper ScientificPaper { get; set; }

        public User User { get; set; }
    }
}
