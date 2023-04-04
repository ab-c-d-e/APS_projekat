using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TheScientistAPI.Model
{
    public class Keyword
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<ScientificPaper> Papers { get; set; }   
    }
}
