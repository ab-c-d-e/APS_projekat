using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class ToDoList
    {
        [Key]
        public int ID { get; set; }

        public ScientificPaper ScientificPaper { get; set; }

        public User User { get; set; }

        public List<ToDoItem> Items { get; set; }
    }
}
