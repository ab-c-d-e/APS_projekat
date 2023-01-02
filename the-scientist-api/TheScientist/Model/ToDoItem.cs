using System.ComponentModel.DataAnnotations;

namespace TheScientist.Model
{
    public class ToDoItem
    {
        [Key]
        public int ID { get; set; }

        public Section Section { get; set; }

        public ToDoList List { get; set; }
    }
}
