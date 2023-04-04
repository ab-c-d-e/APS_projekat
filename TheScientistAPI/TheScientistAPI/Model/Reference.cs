using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheScientistAPI.Model
{
    public class Reference
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Author> Authors { get; set; }
        public string Journal { get; set; }
        public int Year { get; set; }
        public int LinkedPaperId { get; set; }

        [JsonIgnore]
        public List<ScientificPaper> Papers { get; set; }
    }
}
