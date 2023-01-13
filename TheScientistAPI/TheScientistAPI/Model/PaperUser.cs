using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TheScientistAPI.Model
{
    public class PaperUser
    {
        [Key]
        public int ID { get; set; }

        public ScientificPaper ScientificPaper { get; set; }

        public User User { get; set; }

        public ToDoList ToDoList { get; set; }
    }
}
