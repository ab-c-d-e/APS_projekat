using System.ComponentModel.DataAnnotations;

namespace TheScientistAPI.Model
{
    public class ToDoItem
    {
        [Key]
        public int ID { get; set; }

        public string ToDoText { get; set; }

        public Notification Notification { get; set;}

        public ToDoList List { get; set; }
    }
}
